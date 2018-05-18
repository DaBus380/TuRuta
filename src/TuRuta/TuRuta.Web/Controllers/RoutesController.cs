using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TuRuta.Common.Models;
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

        [HttpGet("{name}")]
        public async Task<RouteVM> Get(string name)
            => await _routesService.GetRoute(name);
             
        [HttpGet("[action]/{hint}")]
        public async Task<List<string>> Find(string hint)
            => await _routesService.FindByName(hint);

        [HttpGet("[action]")]
        public async Task<List<string>> Names()
            => await _routesService.GetAllNames();

        [HttpGet("[action]/{name}")]
        public async Task<RouteVM> Create(string name)
            => await _routesService.Create(name);

        [HttpPatch]
        public async Task<RouteVM> Patch([FromBody]RouteVM newRoute)
            => await _routesService.Update(newRoute);

        [HttpPost("[action]/{id}")]
        public async Task<RouteVM> AddStops(string id, [FromBody]List<Guid> stops)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            if(Guid.TryParse(id, out var Id))
            {
                return await _routesService.AddStops(Id, stops);
            }

            return null;
        }

        [HttpGet("[action]/{id}/{stopId}")]
        public async Task<RouteVM> AddStop(string id, string stopId)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            if(Guid.TryParse(id, out var Id) && Guid.TryParse(stopId, out var stop))
            {
                return await _routesService.AddStop(Id, stop);
            }

            return null;
        }
    }
}
