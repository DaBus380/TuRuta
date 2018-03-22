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
			throw new NotImplementedException();
		}

		public Task<List<string>> FindByStops(string hint)
		{
			throw new NotImplementedException();
		}

		public Task<List<StopVM>> GetAllStops()
		{
			throw new NotImplementedException();
		}

		public Task<StopVM> GetStop(string name)
		{
			throw new NotImplementedException();
		}
	}
}
