using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;
using TuRuta.Orleans.Grains.Services.Interfaces;
using TuRuta.Orleans.Grains.Services;
using TuRuta.Orleans.Interfaces;
using TuRuta.Common.Device;
using TuRuta.Orleans.Grains.States;
using Orleans.Providers;
using TuRuta.Common.Models;

namespace TuRuta.Orleans.Grains
{
	[StorageProvider(ProviderName = "AzureTableStore")]
	[ImplicitStreamSubscription("Routes")]
	public class RouteGrain : Grain<RouteState>, IRouteGrain
    {
		private IAsyncStream<Object> injestionStream;

		public Task<List<Stop>> Stops()
		{
			return Task.FromResult(State.Stops);
		}

		public async override Task OnActivateAsync()
		{
			var streamProvider = GetStreamProvider("StreamProvider");
			injestionStream = streamProvider.GetStream<Object>(this.GetPrimaryKey(), "Routes");
			await base.OnActivateAsync();
		}
    }
}
