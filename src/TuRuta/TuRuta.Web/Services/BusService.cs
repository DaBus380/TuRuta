using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return noConfigBusesDb.GetAllValues();
        }
    }
}
