using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Orleans;
using Orleans.Streams;
using Orleans.Providers;
using PubNubMessaging.Core;

using TuRuta.Orleans.Grains.Services.Interfaces;
using TuRuta.Orleans.Grains.Services;
using TuRuta.Orleans.Interfaces;
using TuRuta.Common.Device;
using TuRuta.Common.StreamObjects;
using TuRuta.Orleans.Grains.States;
using TuRuta.Common.Models;
using TuRuta.Common;
using TuRuta.Common.ViewModels;
using TuRuta.Orleans.Grains.Extensions;

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
            Paradas = (await routeGrain.Stops()).Select(stop => stop.ToStop()).ToList() ?? new List<Stop>();

            await GetStreams();

            await base.OnActivateAsync();
        }

        private async Task GetStreams()
        {
            var streamProvider = GetStreamProvider("StreamProvider");
            injestionStream = streamProvider.GetStream<RouteBusUpdate>((this).GetPrimaryKey(), "Buses");
            await injestionStream.SubscribeAsync(this);

            if (State.RouteId != Guid.Empty)
            {
                RouteStream = streamProvider.GetStream<BusRouteUpdate>(State.RouteId, "Routes");
            }
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
            if (State.RouteId == Guid.Empty)
            {
                var noRoutes = GrainFactory.GetGrain<IKeyMapperGrain>(Constants.NoRouteConfiguredGrainName);
                await noRoutes.SetName(this.GetPrimaryKey().ToString(), DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            }

            if (string.IsNullOrEmpty(State.Plates))
            {
                var noConfig = GrainFactory.GetGrain<IKeyMapperGrain>(Constants.NoConfigGrainName);
                await noConfig.SetName(this.GetPrimaryKey().ToString(), DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            }

            if(Paradas.Count() == 0 && State.RouteId != Guid.Empty)
            {
                var routeGrain = GrainFactory.GetGrain<IRouteGrain>(State.RouteId);
                Paradas = (await routeGrain.Stops()).Select(stop => stop.ToStop());
            }

            NextStop = GetClosest(message);

            var sentTask = _clientUpdate.SentUpdate(new ClientBusUpdate
            {
                Location = message.Location,
                BusId = this.GetPrimaryKey(),
                NextStop = NextStop
            }, this.GetPrimaryKey(), OnPubnubError);

            State.Location = message.Location;

            var sendToRoute = RouteStream?.OnNextAsync(new BusRouteUpdate
            {
                BusId = this.GetPrimaryKey(),
                Position = message.Location
            });

            var successful = await sentTask;
            if (sendToRoute != null)
            {
                await sendToRoute;
            }

            if (!successful)
            {
                notSentUpdates.Enqueue(message);
            }

            await WriteStateAsync();
        }

        public Task OnNextAsync(RouteBusUpdate item, StreamSequenceToken token = null)
            => NewPositionReceived(item);

        public Task OnCompletedAsync() => Task.CompletedTask;

        public Task OnErrorAsync(Exception ex) => throw ex;

        public Task<BusVM> GetBusVM()
            => Task.FromResult(new BusVM
            {
                LicensePlate = State.Plates,
                Id = this.GetPrimaryKey(),
                Status = 0,
                Location = State.Location
            });

        public Task SetRoute(Guid route)
        {
            State.RouteId = route;

            var noRoutes = GrainFactory.GetGrain<IKeyMapperGrain>(Constants.NoRouteConfiguredGrainName);
            return Task.WhenAll(GetStreams(), noRoutes.RemoveKey(this.GetPrimaryKey().ToString()), WriteStateAsync());
        }

        public Task SetPlates(string plates)
        {
            State.Plates = plates;

            var platesGrain = GrainFactory.GetGrain<IKeyMapperGrain>(Constants.BusPlatesGrainName);
            var noPlatesGrain = GrainFactory.GetGrain<IKeyMapperGrain>(Constants.NoConfigGrainName);

            var grainId = this.GetPrimaryKey().ToString();
            return Task.WhenAll(platesGrain.SetName(grainId, plates), noPlatesGrain.RemoveKey(grainId), WriteStateAsync());
        }

        private void OnPubnubError(PubnubClientError error)
        {
            return;
        }
            //=> _logger.LogCritical($"Pubnub Error on channel {error.Channel}: {error.Description}"); 
    }
}
