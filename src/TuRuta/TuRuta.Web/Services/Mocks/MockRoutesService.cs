using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;
using TuRuta.Web.Services.Interfaces;
using TuRuta.Common.Models;

namespace TuRuta.Web.Services.Mocks
{
    public class MockRoutesService : IRoutesService
    {
        private RouteVM MockRoute { get; set; } = new RouteVM() 
        {   
            Name = "629",

            Stops = new List<StopVM>()
            {
                new StopVM 
                {
                    Name = "Plaza Galerias",
                    Location = new Point {
                        Latitude = 20.680699,
                        Longitude = -103.430508
                    }
                },
                new StopVM 
                {
                    Name = "Patria y Vallarta",
                    Location = new Point {
                        Latitude = 20.678855,
                        Longitude = -103.422062
                    }
                },
                new StopVM 
                {
                    Name = "Los Cubos",
                    Location = new Point {
                        Latitude = 20.676873,
                        Longitude = -103.412932
                    }
                }
            },
            Buses = new List<BusVM>()
            {
                new BusVM
                {
                    LicensePlate = "MP-18-11",
                    Status = 1,
                    Location = new Point
                    {
                        Latitude = 20.786873,
                        Longitude = -103.522932
                    },
                    Id = Guid.NewGuid()
                }
            }
        };

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

        public Task<RouteVM> Update(RouteVM newRoute)
        {
            MockRoute = newRoute;

            return Task.FromResult(MockRoute);
        }
    }
}
