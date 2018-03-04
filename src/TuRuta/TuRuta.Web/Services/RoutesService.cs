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
        private IKeyMapperGrain _routeDB;
        private IClusterClient _clusterClient { get; }
        public RoutesService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _routeDB = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.RouteGrainName);
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

        public async Task<RouteVM> AddStops(Guid id, List<CreateStopVM> stops)
        {
            var routeGrain = _clusterClient.GetGrain<IRouteGrain>(id);
            await routeGrain.AddStops(stops);

            return await routeGrain.GetRouteVM();
        }

        public async Task<RouteVM> AddStop(Guid id, CreateStopVM stop)
        {
            var routeGrain = _clusterClient.GetGrain<IRouteGrain>(id);
            await routeGrain.AddStop(stop);

            return await routeGrain.GetRouteVM();
        }

        public Task<List<string>> FindByName(string hint)
            => _routeDB.FindByValue(hint);
    }
}
