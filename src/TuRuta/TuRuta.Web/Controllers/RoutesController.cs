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
        public Task<RouteVM> Get(string name)
            => _routesService.GetRoute(name);
             
        [HttpGet("[action]/{hint}")]
        public Task<List<string>> Find(string hint)
            => _routesService.FindByName(hint);

        [HttpGet("[action]")]
        public Task<List<string>> Names()
            => _routesService.GetAllNames();

        [HttpGet("[action]/{name}")]
        public Task<RouteVM> Create(string name)
            => _routesService.Create(name);

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

        [HttpPost("[action]/{id}")]
        public async Task<RouteVM> AddStop(string id, [FromBody]Guid stop)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            if(Guid.TryParse(id, out var Id))
            {
                return await _routesService.AddStop(Id, stop);
            }

            return null;
        }
    }
}
