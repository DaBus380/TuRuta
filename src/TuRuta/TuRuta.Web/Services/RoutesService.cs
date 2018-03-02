using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TuRuta.Common;
using TuRuta.Common.ViewModels;
using TuRuta.Orleans.Interfaces;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Services
{
    public class RoutesService : IRoutesService
    {
        private IClusterClient _clusterClient { get; }
        public RoutesService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public async Task<RouteVM> Create(string name)
        {
            var Id = Guid.NewGuid();

            var routeDb = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.RouteGrainName);
            await routeDb.SetName(name, Id.ToString());

            return default(RouteVM);
        }
    }
}
