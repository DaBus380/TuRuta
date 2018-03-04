using Microsoft.Extensions.Configuration;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TuRuta.Common.ViewModels.ConfigVMs;
using TuRuta.Orleans.Interfaces;
using TuRuta.Web.Services.Interfaces;
using TuRuta.Common;

namespace TuRuta.Web.Services
{
    public class ConfigService : IConfigService
    {
        private readonly string QueueName;
        private readonly string ServiceBusConnectionString;
        private readonly string PubnubSub;
        private readonly string PubnubPub;

        private IClusterClient _clusterClient { get; }

        public ConfigService(
            IClusterClient clusterClient,
            IConfiguration configuration)
        {
            QueueName = configuration.GetValue<string>(nameof(QueueName));
            ServiceBusConnectionString = configuration.GetValue<string>(nameof(ServiceBusConnectionString));
            PubnubSub = configuration.GetValue<string>(nameof(PubnubSub));
            PubnubPub = configuration.GetValue<string>(nameof(PubnubPub));

            _clusterClient = clusterClient;
        }
        
        public async Task<BusConfigVM> GetConfig(string macAddress)
        {
            var configGrain = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.BusConfigGrainName);
            var id = await configGrain.GetId(macAddress);
            if(Guid.TryParse(id, out var ID))
            {
                return new BusConfigVM
                {
                    BusId = ID,
                    QueueName = QueueName,
                    ServiceBusConnectionString = ServiceBusConnectionString
                };
            }

            return default(BusConfigVM);
        }

        public Task<PubnubConfig> GetPubnub()
            => Task.FromResult(new PubnubConfig
            {
                PubKey = PubnubPub,
                SubKey = PubnubSub
            });
    }
}