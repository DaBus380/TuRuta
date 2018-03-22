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
using TuRuta.Common.ViewModels;

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

        public async Task<List<StopVM>> Stops()
            => (await Task.WhenAll(State.Stops.Select(stop => stop.GetStop()))).ToList();

        public async override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("StreamProvider");
            updateBusStream = streamProvider.GetStream<BusRouteUpdate>(this.GetPrimaryKey(), "Routes");
            await updateBusStream.SubscribeAsync(this);

            await base.OnActivateAsync();
        }

        public Task AddStops(List<IStopGrain> stops)
        {
            State.Stops.AddRange(stops);
            return WriteStateAsync();
        }

        public Task AddStop(IStopGrain stop)
        {
            State.Stops.Add(stop);
            return WriteStateAsync();
        }

        public Task<Guid> GetNearestBus(Point position)
            => Task.FromResult(BusPositions
                .Select(pair => (pair.Key, _calculator.GetDistance(pair.Value, position)))
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

            var bus = GrainFactory.GetGrain<IBusGrain>(busRouteUpdate.BusId);
            if (!State.Buses.Contains(bus))
            {
                State.Buses.Add(bus);
            }

            return Task.CompletedTask;
        }

        public Task SetName(string name)
        {
            State.Name = name;
            return WriteStateAsync();
        }

        public Task OnNextAsync(BusRouteUpdate item, StreamSequenceToken token = null)
            => UpdateBusPosition(item);
        public Task OnCompletedAsync() => Task.CompletedTask;
        public Task OnErrorAsync(Exception ex) => throw new NotImplementedException();

        public async Task<RouteVM> GetRouteVM()
        {
            var resultsTask = Task.WhenAll(State.Buses.Select(bus => bus.GetBusVM()));
            var stopsTask = Task.WhenAll(State.Stops.Select(stop => stop.GetStop()));

            var vm = new RouteVM
            {
                Id = this.GetPrimaryKey(),
                Name = State.Name,
                Incidents = new List<IncidentVM>()
            };

            var buses = await resultsTask;
            var stops = await stopsTask;

            vm.Stops = stops.ToList();
            vm.Buses = buses.ToList();


            return vm;
        }
    }
}
