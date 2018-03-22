using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Orleans;

using TuRuta.Common.ViewModels;
using TuRuta.Orleans.Grains.Extensions;
using TuRuta.Orleans.Grains.States;
using TuRuta.Orleans.Interfaces;

namespace TuRuta.Orleans.Grains
{
	public class StopGrain : Grain<StopState>, IStopGrain
	{
		public Task AddInfo(StopVM stopVM)
		{
			State.Location = stopVM.Location;
			State.Name = stopVM.Name;
		}
        public async Task<List<RouteVM>> GetRoutes()
            => (await Task.WhenAll(State.Routes.Select(route => route.GetRouteVM()))).ToList();
            
		public Task<StopVM> GetStop()
			=> Task.FromResult(State.ToVM(this.GetPrimaryKey()));

		public Task SetRoute(Guid routeId)
		{
			var routeGrain = GrainFactory.GetGrain<IRouteGrain>(routeId);
			State.Routes.Add(routeGrain);
			return Task.CompletedTask;
		}
	}
}
