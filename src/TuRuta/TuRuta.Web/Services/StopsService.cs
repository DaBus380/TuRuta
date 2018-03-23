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

		public async Task<StopVM> CreateStop(StopVM stopVM)
		{
            var stopId = Guid.NewGuid();
            var setName = _stopNameDb.SetName(stopId.ToString(), stopVM.Name);

            var stopGrain = _clusterClient.GetGrain<IStopGrain>(stopId);
            await stopGrain.AddInfo(stopVM);

            return await stopGrain.GetStop();
		}

        public Task<List<string>> FindByStops(string hint)
            => _stopNameDb.FindByValueGetValues(hint);

		public async Task<List<StopVM>> GetAllStops()
		{
            var keys = (await _stopNameDb.GetAllKeys()).Select(key => Guid.Parse(key));

            return (await Task.WhenAll(keys.Select(key =>
            {
                var grain = _clusterClient.GetGrain<IStopGrain>(key);
                return grain.GetStop();
            }))).ToList();
		}

		public async Task<StopVM> GetStop(string name)
		{
            var stopIds = await _stopNameDb.FindByValue(name);
            if(stopIds.Count() != 1)
            {
                return null;
            }

            var stopId = stopIds.First();
            if(Guid.TryParse(stopId, out var StopId))
            {
                var stopGrain = _clusterClient.GetGrain<IStopGrain>(StopId);
                return await stopGrain.GetStop();
            }

            return null;
		}
	}
}
