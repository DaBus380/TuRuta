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
    public class StopsController : Controller
    {
        private IStopService _stopService { get; }
        public StopsController(IStopService stopService)
        {
            _stopService = stopService;
        }

        [HttpPost]
        public async Task<StopVM> Post([FromBody]StopVM stopVM)
            => await _stopService.CreateStop(stopVM);

        [HttpPatch]
        public async Task<StopVM> Patch([FromBody]StopVM stopVM)
            => await _stopService.Update(stopVM);

        [HttpGet]
        public async Task<List<StopVM>> Get()
            => await _stopService.GetAllStops();

        [HttpGet("{name}")]
        public async Task<StopVM> Get(string name)
            => await _stopService.GetStop(name);

        [HttpGet("[action]/{hint}")]
        public async Task<List<string>> Find(string hint)
            => await _stopService.FindByStops(hint);

        [HttpGet("[action]/{id}")]
        public async Task<List<RouteVM>> GetRoutes(string id)
        {
            if(Guid.TryParse(id, out var ID))
            {
                return await _stopService.GetRoutes(ID);
            }

            return null;
        }
    }
}
