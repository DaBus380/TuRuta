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
using TuRuta.Orleans.Grains.States;
using TuRuta.Common.Models;
using TuRuta.Common.StreamObjects;

namespace TuRuta.Orleans.Grains
{
	[StorageProvider(ProviderName = "AzureTableStore")]
	[ImplicitStreamSubscription("Routes")]
	public class RouteGrain : Grain<RouteState>, IRouteGrain, IAsyncObserver<BusRouteUpdate>
    {
        private Dictionary<Guid, Point> BusPositions = new Dictionary<Guid, Point>();
		private IAsyncStream<BusRouteUpdate> updateBusStream;
        private IDistanceCalculator _calculator;

        public RouteGrain(IDistanceCalculator calculator)
        {
            _calculator = calculator;
        }

		public Task<List<Stop>> Stops()
		{
			return Task.FromResult(State.Stops);
		}

		public async override Task OnActivateAsync()
		{
			var streamProvider = GetStreamProvider("StreamProvider");
            updateBusStream = streamProvider.GetStream<BusRouteUpdate>(this.GetPrimaryKey(), "Routes");
            await updateBusStream.SubscribeAsync(this);
            
			await base.OnActivateAsync();
        }

        public Task<Guid> GetNearestBus(Point position)
            => Task.FromResult(BusPositions
                .Select(pair => (pair.Key, _calculator.GetDistance(pair.Value.Latitude, position.Latitude, pair.Value.Longitude, position.Longitude)))
                .OrderByDescending(tuple => tuple.Item2)
                .First().Key);

        private Task UpdateBusPosition(BusRouteUpdate busRouteUpdate)
        {
            if (BusPositions.ContainsKey(busRouteUpdate.BusId))
            {
                BusPositions[busRouteUpdate.BusId] = busRouteUpdate.Position;
                return Task.CompletedTask;
            }

            BusPositions.Add(busRouteUpdate.BusId, busRouteUpdate.Position);

            return Task.CompletedTask;
        }

        public Task OnNextAsync(BusRouteUpdate item, StreamSequenceToken token = null)
            => UpdateBusPosition(item);

        public Task OnCompletedAsync() => throw new NotImplementedException();
        public Task OnErrorAsync(Exception ex) => throw new NotImplementedException();
    }
}
