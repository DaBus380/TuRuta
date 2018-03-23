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
        public Task<StopVM> Post(StopVM stopVM)
            => _stopService.CreateStop(stopVM);

        [HttpGet]
        public Task<List<StopVM>> Get()
            => _stopService.GetAllStops();

        [HttpGet]
        public Task<StopVM> Get(string name)
            => _stopService.GetStop(name);

        [HttpGet("/{action}/{hint}")]
        public Task<List<string>> Find(string hint)
            => _stopService.FindByStops(hint);
    }
}
