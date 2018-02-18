using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;
using Orleans.Providers;
using System.Linq;

using TuRuta.Orleans.Grains.Services.Interfaces;
using TuRuta.Orleans.Grains.Services;
using TuRuta.Orleans.Interfaces;
using TuRuta.Common.Device;
using TuRuta.Common.StreamObjects;
using TuRuta.Orleans.Grains.States;
using TuRuta.Common.Models;

namespace TuRuta.Orleans.Grains
{
    [StorageProvider(ProviderName = "AzureTableStore")]
    [ImplicitStreamSubscription("Buses")]
    public class BusGrain : Grain<BusState>, IBusGrain
    {
        private IAsyncStream<PositionUpdate> injestionStream;
		private IClientUpdate clientUpdate;
        private IAsyncStream<object> RouteStream;
		private Queue<PositionUpdate> notSentUpdates = new Queue<PositionUpdate>();
        private IDistanceCalculator distanceCalculator;
        private IEnumerable<Stop> Paradas;
        private Stop NextStop;

        public async override Task OnActivateAsync()
        {
            Paradas = new List<Stop>();
            var configClient = new ConfigClient();
            var config = await configClient.GetPubnubConfig();

			clientUpdate = new PubNubClientUpdate(config.SubKey, config.PubKey);
            distanceCalculator = new HavesineDistanceCalculator();

            var routeGrain = GrainFactory.GetGrain<IRouteGrain>(State.RouteId);
            Paradas = await routeGrain.Stops();

            await GetStreams();

            await base.OnActivateAsync();
        }

        private async Task GetStreams()
        {
            var streamProvider = GetStreamProvider("StreamProvider");
            injestionStream = streamProvider.GetStream<PositionUpdate>(this.GetPrimaryKey(), "Buses");
            await injestionStream.SubscribeAsync(NewPositionReceived);

            RouteStream = streamProvider.GetStream<object>(State.RouteId, "Rutas");
        }

        private Stop GetClosest(PositionUpdate message)
            => Paradas.Select(
                parada => (Distance: distanceCalculator.GetDistance(
                    message.Latitude,
                    parada.Latitude,
                    message.Longitude,
                    parada.Longitude), Parada: parada))
                .OrderByDescending(tuple => tuple.Distance)
                .FirstOrDefault().Parada;

        private async Task NewPositionReceived(PositionUpdate message, StreamSequenceToken token)
        {
            NextStop = GetClosest(message);

            var sentTask = clientUpdate.SentUpdate(new ClientBusUpdate
            {
                Latitude = message.Latitude,
                Longitude = message.Longitude,
                BusId = this.GetPrimaryKey(),
                NextStop = new Stop
                {
                    Latitude = 123.43,
                    Longitude = 45.32,
                    Name = "Plaza Galerias"
                }
            });

            State.CurrentLatitude = message.Latitude;
            State.CurrentLongitude = message.Longitude;
            
            var successful = await sentTask;

            if (!successful)
            {
                notSentUpdates.Enqueue(message);
            }
        }
    }
}
