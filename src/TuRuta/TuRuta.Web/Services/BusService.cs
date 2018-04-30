using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Orleans;

using TuRuta.Common;
using TuRuta.Orleans.Interfaces;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Services
{
    public class BusService : IBusService
    {
        private IClusterClient _clusterClient { get; }
        public BusService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public Task<List<string>> FindBusByPlates(string plates)
        {
            var busPlateDb = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.BusPlatesGrainName);
            return busPlateDb.FindByValue(plates);
        }

        public Task<List<string>> GetNoConfiguredBuses()
        {
            var noConfigBusesDb = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.NoRouteConfiguredGrainName);
            return noConfigBusesDb.GetAllKeys();
        }

        public Task<List<string>> GetNoConfiguredPlates()
        {
            var noConfiguredPlates = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.NoConfigGrainName);
            return noConfiguredPlates.GetAllKeys();
        }

        public Task SetRoute(Guid busId, Guid routeId)
        {
            var busGrain = _clusterClient.GetGrain<IBusGrain>(busId);
            return busGrain.SetRoute(routeId);
        }

        public Task SetPlates(Guid busId, string plates)
        {
            var busGrain = _clusterClient.GetGrain<IBusGrain>(busId);
            return busGrain.SetPlates(plates);
        }
    }
}
