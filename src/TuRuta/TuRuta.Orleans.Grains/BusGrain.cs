using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Orleans;
using Orleans.Streams;
using Orleans.Providers;

using TuRuta.Orleans.Grains.Services.Interfaces;
using TuRuta.Orleans.Grains.Services;
using TuRuta.Orleans.Interfaces;
using TuRuta.Common.Device;
using TuRuta.Common.StreamObjects;
using TuRuta.Orleans.Grains.States;
using TuRuta.Common.Models;
using Microsoft.Extensions.Logging;

namespace TuRuta.Orleans.Grains
{
    [StorageProvider(ProviderName = "AzureTableStore")]
    [ImplicitStreamSubscription("Buses")]
    public class BusGrain : Grain<BusState>, IBusGrain, IAsyncObserver<RouteBusUpdate>
    {
        private IAsyncStream<RouteBusUpdate> injestionStream;
        private IClientUpdate _clientUpdate;
        private IAsyncStream<BusRouteUpdate> RouteStream;
        private Queue<RouteBusUpdate> notSentUpdates = new Queue<RouteBusUpdate>();
        private IDistanceCalculator _distanceCalculator;
        private IEnumerable<Stop> Paradas;
        private Stop NextStop;
        private IConfigClient _configClient;

        public BusGrain(
            IDistanceCalculator distanceCalculator,
            IConfigClient configClient)
        {
            _distanceCalculator = distanceCalculator;
            _configClient = configClient;
        }

        public async override Task OnActivateAsync()
        {
            var config = await _configClient.GetPubnubConfig();

			_clientUpdate = new PubNubClientUpdate(config.SubKey, config.PubKey);

            var routeGrain = GrainFactory.GetGrain<IRouteGrain>(State.RouteId);
            Paradas = await routeGrain.Stops() ?? new List<Stop>();

            await GetStreams();

            await base.OnActivateAsync();
        }

        private async Task GetStreams()
        {
            var streamProvider = GetStreamProvider("StreamProvider");
            injestionStream = streamProvider.GetStream<RouteBusUpdate>((this).GetPrimaryKey(), "Buses");
            await injestionStream.SubscribeAsync(this);

            RouteStream = streamProvider.GetStream<BusRouteUpdate>(State.RouteId, "Routes");
        }

        private Stop GetClosest(RouteBusUpdate message)
            => Paradas.Select(
                parada => (Distance: _distanceCalculator.GetDistance(
                    message.Location,
                    State.Location), Parada: parada))
                .OrderByDescending(tuple => tuple.Distance)
                .FirstOrDefault().Parada;

        private async Task NewPositionReceived(RouteBusUpdate message)
        {
            NextStop = GetClosest(message);

            var sentTask = _clientUpdate.SentUpdate(new ClientBusUpdate
            {
                Location = message.Location,
                BusId = this.GetPrimaryKey(),
                NextStop = new Stop
                {
                    Location = message.Location,
                    Name = "Plaza Galerias"
                }
            });

            State.Location = message.Location;

            var routeUpdate = RouteStream.OnNextAsync(new BusRouteUpdate
            {
                BusId = this.GetPrimaryKey(),
                Position = message.Location
            });
            
            var successful = await sentTask;
            await routeUpdate;

            if (!successful)
            {
                notSentUpdates.Enqueue(message);
            }
        }

        public Task OnNextAsync(RouteBusUpdate item, StreamSequenceToken token = null)
            => NewPositionReceived(item);

        public Task OnCompletedAsync() => Task.CompletedTask;

        public Task OnErrorAsync(Exception ex) => throw ex;
    }
}
