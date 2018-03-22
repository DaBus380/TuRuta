using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Services.Mocks
{
    public class MockBusService : IBusService
    {
        private List<string> Lists = new List<string>
        {
            Guid.NewGuid().ToString()
        };

        public Task<List<string>> FindBusByPlates(string plates) => Task.FromResult(Lists);
        public Task<List<string>> GetNoConfiguredBuses() => Task.FromResult(Lists);
        public Task<List<string>> GetNoConfiguredPlates() => Task.FromResult(Lists);
        
        public Task SetPlates(Guid busId, string plates) => Task.CompletedTask;
        public Task SetRoute(Guid busId, Guid routeId) => Task.CompletedTask;
    }
}
