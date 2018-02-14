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
    [ImplicitStreamSubscription("Rutas")]
    [StorageProvider(ProviderName = "AzureTableStore")]
	public class RutaGrain : Grain<RutaState>, IRutaGrain
    {
		private IAsyncStream<object> injestionStreamParada;

        public Task<List<Parada>> AllParadas()
            => Task.FromResult(State.AllParadas);

        public async override Task OnActivateAsync()
		{
			var streamProvider = GetStreamProvider("StreamProvider");
			injestionStreamParada = streamProvider.GetStream<object>(this.GetPrimaryKey(), "Rutas");
                
			await base.OnActivateAsync();
		}
    }
}
