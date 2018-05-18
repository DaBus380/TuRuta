using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;
using Orleans;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;

namespace TuRuta.Orleans.Interfaces
{
    public interface IRouteGrain : IGrainWithGuidKey
    {
        Task ClearStops();
        Task SetName(string name);
		Task<List<StopVM>> Stops();
        Task<Guid> GetNearestBus(Point position);
        Task AddStops(List<IStopGrain> stops);
        Task AddStop(IStopGrain stop);
        Task<RouteVM> GetRouteVM();
        Task<RouteVM> GetRouteInfo();
    }
}
