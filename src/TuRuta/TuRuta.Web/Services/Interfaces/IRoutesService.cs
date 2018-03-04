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
        Task<List<string>> FindByName(string hint);

        Task<RouteVM> Create(string name);

        Task<List<string>> GetAllNames();

        Task<RouteVM> AddStops(Guid id, List<CreateStopVM> stops);

        Task<RouteVM> AddStop(Guid id, CreateStopVM stop);
    }
}
