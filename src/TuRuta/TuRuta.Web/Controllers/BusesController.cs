using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;
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
        public Task<List<string>> NoPlates()
            => _busService.GetNoConfiguredPlates();

        [HttpGet("[action]")]
        public Task<List<string>> NoConfigured()
            => _busService.GetNoConfiguredBuses();

        [HttpGet("[action]/{plates}")]
        public async Task<List<string>> Find(string plates)
            => await _busService.FindBusByPlates(plates);

        [HttpGet("[action]/{busId}/{routeId}")]
        public async Task<IActionResult> SetRoute(string busId, string routeId)
        {
            if(Guid.TryParse(busId, out var BusId) && Guid.TryParse(routeId, out var RouteId))
            {
                await _busService.SetRoute(BusId, RouteId);
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("[action]/{busId}/{plates}")]
        public async Task<IActionResult> SetPlates(string busId, string plates)
        {
            if(Guid.TryParse(busId, out var BusId))
            {
                await _busService.SetPlates(BusId, plates);
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
		public async Task<BusInfoVM> Get(string id)
		{
			if (Guid.TryParse(id, out var ID))
			{
				return await _busService.GetBus(ID);
			}
			return null;
		}
    }
}
