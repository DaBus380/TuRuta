using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;
using TuRuta.Web.Services.Interfaces;
using TuRuta.Common.Models;

namespace TuRuta.Web.Services.Mocks
{
    public class MockStopService : IStopService
    {
        private StopVM Stop { get; set; } = new StopVM()
        {
            Name = "La Minerva",
            Location = new Point {
                Latitude = 20.680699,
                Longitude = -103.430508
            }
        };

        public Task<StopVM> CreateStop(StopVM stopVM)
        {
            Stop.Name = stopVM.Name;
            Stop.Location = stopVM.Location;
            Stop.Id = Guid.NewGuid();

            return Task.FromResult(Stop);
        }

        public Task<List<string>> FindByStops(string hint)
            => Task.FromResult(new List<string>
            {
                Stop.Name
            });

        public Task<List<StopVM>> GetAllStops()
            => Task.FromResult(new List<StopVM>
            {
                Stop
            });

        public Task<List<RouteVM>> GetRoutes(Guid id)
            => Task.FromResult(new List<RouteVM>
            {
                new RouteVM
                {
                    Name = "380-B",
                    Id = Guid.NewGuid()
                }
            });

        public Task<StopVM> GetStop(string name)
            => Task.FromResult(Stop);

        public Task<StopVM> Update(StopVM stopVM)
        {
            Stop = stopVM;
            return Task.FromResult(Stop);
        }
    }
}
