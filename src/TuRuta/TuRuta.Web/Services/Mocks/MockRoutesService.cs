using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Services.Mocks
{
    public class MockRoutesService : IRoutesService
    {
        private RouteVM MockRoute { get; } = new RouteVM();

        public Task<RouteVM> AddStop(Guid id, Guid stop)
        {
            MockRoute.Stops.Add(new StopVM
            {
                Id = stop
            });

            return Task.FromResult(MockRoute);
        }

        public Task<RouteVM> AddStops(Guid id, List<Guid> stops)
        {
            MockRoute.Stops.AddRange(stops.Select(stopId => new StopVM
            {
                Id = stopId
            }));

            return Task.FromResult(MockRoute);
        }

        public Task<RouteVM> Create(string name)
        {
            MockRoute.Name = name;
            return Task.FromResult(MockRoute);
        }

        public Task<List<string>> FindByName(string hint)
            => Task.FromResult(new List<string>
            {
                MockRoute.Name
            });

        public Task<List<string>> GetAllNames() 
            => Task.FromResult(new List<string>
            {
                MockRoute.Name
            });

        public Task<RouteVM> GetRoute(string name)
            => Task.FromResult(MockRoute);
    }
}
