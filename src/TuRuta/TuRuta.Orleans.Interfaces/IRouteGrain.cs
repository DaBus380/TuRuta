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
        Task SetName(string name);
		Task<List<Stop>> Stops();
        Task<Guid> GetNearestBus(Point position);
        Task AddStops(List<CreateStopVM> stops);
        Task AddStop(CreateStopVM stop);
        Task<RouteVM> GetRouteVM();
    }
}
