using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.Models;
using TuRuta.Common.ViewModels;

namespace TuRuta.Web.Services.Interfaces
{
    public interface IRoutesService
    {
        Task<RouteVM> GetRoute(string name);

        Task<List<string>> FindByName(string hint);

        Task<RouteVM> Create(string name);

        Task<List<string>> GetAllNames();

        Task<RouteVM> AddStops(Guid id, List<Guid> stops);

        Task<RouteVM> AddStop(Guid id, Guid stop);

        Task<RouteVM> Update(RouteVM newRoute);
    }
}
