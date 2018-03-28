using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using TuRuta.Common;
using TuRuta.Common.ViewModels;
using TuRuta.Orleans.Interfaces;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Services
{
	public class StopsService : IStopService
	{
		private IClusterClient _clusterClient { get; }
		private IKeyMapperGrain _stopNameDb { get; }
		public StopsService(IClusterClient clusterClient)
		{
			_clusterClient = clusterClient;
			_stopNameDb = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.StopGrainName);
		}
		public Task<StopVM> CreateStop(StopVM stopVM)
		{
			var stop = new StopVM
			{
				Id = stopVM.Id,
				Location = stopVM.Location,
				Name = stopVM.Name
			};
			return Task.FromResult(stop);
		}

		public Task<List<string>> FindByStops(string hint)
			=> _stopNameDb.FindByValueGetValues(hint);

		public Task<List<StopVM>> GetAllStops()
		{

			
		}

		public async Task<StopVM> GetStop(string name)
		{
			var foundIds = await _stopNameDb.FindByValue(name);
			if (foundIds.Count != 1)
			{
				return null;
			}

			var id = Guid.Parse(foundIds.First());
			var stop = _clusterClient.GetGrain<IStopGrain>(id);
			return await stop.GetStopVM();
		}
	}
}
