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
        private IKeyMapperGrain _configDb { get; }
        private IKeyMapperGrain _noConfigDb { get; }

        public ConfigService(
            IClusterClient clusterClient,
            IConfiguration configuration)
        {
            QueueName = configuration.GetValue<string>(nameof(QueueName));
            ServiceBusConnectionString = configuration.GetValue<string>(nameof(ServiceBusConnectionString));
            PubnubSub = configuration.GetValue<string>(nameof(PubnubSub));
            PubnubPub = configuration.GetValue<string>(nameof(PubnubPub));

            _clusterClient = clusterClient;
            _configDb = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.BusConfigGrainName);
            _noConfigDb = _clusterClient.GetGrain<IKeyMapperGrain>(Constants.NoRouteConfiguredGrainName);
        }

        public async Task<BusConfigVM> GetConfig(string macAddress)
        {
            var config = new BusConfigVM
            {
                QueueName = QueueName,
                ServiceBusConnectionString = ServiceBusConnectionString
            };

            var id = await _configDb.GetId(macAddress);
            if(Guid.TryParse(id, out var ID))
            {
                config.BusId = ID;
                return config;
            }

            var newId = Guid.NewGuid();
            await _configDb.SetName(macAddress, newId.ToString());
            config.BusId = newId;

            return config;
        }

        public Task<List<string>> GetNoConfig()
            => _noConfigDb.GetAllKeys();

        public Task<PubnubConfig> GetPubnub()
            => Task.FromResult(new PubnubConfig
            {
                PubKey = PubnubPub,
                SubKey = PubnubSub
            });
    }
}