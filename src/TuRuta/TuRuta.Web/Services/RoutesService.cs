using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TuRuta.Common;
using TuRuta.Common.Models;
using TuRuta.Common.ViewModels;
using TuRuta.Orleans.Interfaces;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Services
{
    public class RoutesService : IRoutesService
    {
        private IKeyMapperGrain _stopNameDb { get; }
        private IKeyMapperGrain _routeDB { get; }
        private IClusterClient _clusterClient { get; }
        public RoutesService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _routeDB = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.RouteGrainName);
            _stopNameDb = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.StopGrainName);
        }

        public async Task<RouteVM> Create(string name)
        {
            var Id = Guid.NewGuid();

            var routeGrain = _clusterClient.GetGrain<IRouteGrain>(Id);

            var setNameTask = _routeDB.SetName(Id.ToString(), name);
            var addNameTask = routeGrain.SetName(name);

            await Task.WhenAll(setNameTask, addNameTask);

            return new RouteVM
            {
                Id = Id,
                Name = name
            };
        }

        public Task<List<string>> GetAllNames()
            => _routeDB.GetAllValues();

        public async Task<RouteVM> AddStops(Guid id, List<Guid> stops)
        {
            var routeGrain = _clusterClient.GetGrain<IRouteGrain>(id);
            var grains = stops
                .Select(stopId => _clusterClient.GetGrain<IStopGrain>(stopId))
                .ToList();

            await routeGrain.AddStops(grains);

            return await routeGrain.GetRouteVM();
        }

        public async Task<RouteVM> AddStop(Guid id, Guid stopId)
        {
            var routeGrain = _clusterClient.GetGrain<IRouteGrain>(id);
            var stopGrain = _clusterClient.GetGrain<IStopGrain>(stopId);
            await routeGrain.AddStop(stopGrain);

            return await routeGrain.GetRouteVM();
        }

        public Task<List<string>> FindByName(string hint)
            => _routeDB.FindByValueGetValues(hint);

        public async Task<RouteVM> GetRoute(string name)
        {
            var foundIds = await _routeDB.FindByValue(name);
            if(foundIds.Count != 1)
            {
                return null;
            }

            var id = Guid.Parse(foundIds.First());
            var route = _clusterClient.GetGrain<IRouteGrain>(id);
            return await route.GetRouteVM();
        }

        public async Task<RouteVM> Update(RouteVM newRoute)
        {
            var route = _clusterClient.GetGrain<IRouteGrain>(newRoute.Id);
            var name = _routeDB.FindByKey(newRoute.Id.ToString());

            await route.SetName(newRoute.Name);
            await route.ClearStops();
            await route.AddStops(newRoute.Stops.Select(stop => _clusterClient.GetGrain<IStopGrain>(stop.Id)).ToList());

            var results = await name;
            if(results.Count != 1)
            {
                return null;
            }

            if (!results.First().Equals(newRoute.Name))
            {
                await _routeDB.UpdateKey(newRoute.Id.ToString(), newRoute.Name);
            }

            return await route.GetRouteVM();
        }
    }
}
