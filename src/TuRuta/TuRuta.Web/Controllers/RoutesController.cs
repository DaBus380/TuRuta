using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TuRuta.Common.ViewModels;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Controllers
{
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {
        private IRoutesService _routesService { get; }
        public RoutesController(IRoutesService routesService)
        {
            _routesService = routesService;
        }

        [HttpPost]
        public Task<RouteVM> Post()
            => _routesService.Create("Holis");

        [HttpGet]
        public IEnumerable<RouteVM> Get()
        {
            return Enumerable.Range(0, 5).Select(index =>
                new RouteVM
                {
                    Buses = Enumerable.Range(0, 1).Select(i =>
                        new BusVM
                        {
                            Id = Guid.NewGuid(),
                            LicensePlate = "RD-D2",
                            Status = i
                        }
                    ).ToList(),
                    Id = Guid.NewGuid(),
                    Incidents = Enumerable.Range(0, 2).Select(j =>
                        new IncidentVM
                        {
                            Description = "do",
                            Id = Guid.NewGuid(),
                            Issue = j,
                            Name = "holi"
                        }
                    ).ToList(),
                    Name = "380-A",
                    Stops = Enumerable.Range(0, 3).Select(k =>
                        new StopVM
                        {
                            Id = Guid.NewGuid(),
                            Location = (k, k),
                            Name = "Av. Vallarta"
                        }
                    ).ToList()
                }
            ).ToList();
        }
    }
}
