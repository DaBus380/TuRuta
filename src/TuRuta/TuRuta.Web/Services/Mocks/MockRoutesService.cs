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

        public Task<RouteVM> AddStop(Guid id, CreateStopVM stop)
        {
            MockRoute.Stops.Add(new StopVM
            {
                Id = Guid.NewGuid(),
                Location= stop.Location,
                Name = stop.Name
            });

            return Task.FromResult(MockRoute);
        }

        public Task<RouteVM> AddStops(Guid id, List<CreateStopVM> stops)
        {
            MockRoute.Stops.AddRange(stops.Select(stop => new StopVM
            {
                Id = Guid.NewGuid(),
                Location = stop.Location,
                Name = stop.Name
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
