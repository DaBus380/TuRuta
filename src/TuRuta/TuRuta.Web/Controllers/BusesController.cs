using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Controllers
{
    [Route("api/[controller]")]
    public class BusesController : Controller
    {
        private IBusService _busService { get; }
        public BusesController(IBusService busService)
        {
            _busService = busService;
        }

        [HttpGet("[action]")]
        public Task<List<string>> NoConfigured()
            => _busService.GetNoConfiguredBuses();

        [HttpGet("[action]/{plates}")]
        public async Task<List<string>> Find(string plates)
            => await _busService.FindBusByPlates(plates);
    }
}
