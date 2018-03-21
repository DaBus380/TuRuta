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

        public Task AddStops(List<CreateStopVM> stops)
        {
            State.Stops.AddRange(stops.Select(stop => new Stop {
                Id = Guid.NewGuid(),
                Location = stop.Location,
                Name = stop.Name
            }));
            return WriteStateAsync();
        }

        public Task AddStop(CreateStopVM stop)
        {
            State.Stops.Add(new Stop
            {
                Id = Guid.NewGuid(),
                Location = stop.Location,
                Name = stop.Name
            });
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

            var vm = new RouteVM
            {
                Id = this.GetPrimaryKey(),
                Name = State.Name,
                Stops = State.Stops.Select(stop => new StopVM
                {
                    Id = stop.Id,
                    Location = stop.Location,
                    Name = stop.Name
                })
                .ToList(),
                Incidents = new List<IncidentVM>()
            };

            var buses = await resultsTask;
            vm.Buses = buses.ToList();


            return vm;
        }
    }
}
