using System;
using System.Collections.Generic;
using System.Text;
using TuRuta.Common.Models;
using Orleans;
using System.Threading.Tasks;

namespace TuRuta.Orleans.Interfaces
{
    public interface IRouteGrain : IGrainWithGuidKey
    {
		Task<List<Stop>> Stops();
        Task<Guid> GetNearestBus(Point position);
    }
}
