using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using TuRuta.Common.ViewModels;

namespace TuRuta.Orleans.Interfaces
{
    public interface IStopGrain : IGrainWithGuidKey
    {
		Task AddInfo(StopVM stopVM);
		Task<StopVM> GetStop();
		Task<List<RouteVM>> GetRoutes();
		Task SetRoute(Guid routeId);
    }
}
