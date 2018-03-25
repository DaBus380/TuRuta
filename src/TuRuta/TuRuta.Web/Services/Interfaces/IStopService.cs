using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;

namespace TuRuta.Web.Services.Interfaces
{
    public interface IStopService
    {
		Task<StopVM> GetStop(string name);
		Task<StopVM> CreateStop(StopVM stopVM);
		Task<List<string>> FindByStops(string hint);
		Task<List<StopVM>> GetAllStops();
        Task<List<RouteVM>> GetRoutes(Guid id);
    }
}
