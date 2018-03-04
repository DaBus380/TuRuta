using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TuRuta.Web.Services.Interfaces;
using TuRuta.Common.ViewModels.ConfigVMs;

namespace TuRuta.Web.Services.Mocks
{
    public class MockConfigService : IConfigService
    {
        private readonly string QueueName;
        private readonly string ServiceBusConnectionString;
        private readonly string PubnubSub;
        private readonly string PubnubPub;

        public MockConfigService(IConfiguration configuration)
        {
            QueueName = configuration.GetValue<string>(nameof(QueueName));
            ServiceBusConnectionString = configuration.GetValue<string>(nameof(ServiceBusConnectionString));
            PubnubSub = configuration.GetValue<string>(nameof(PubnubSub));
            PubnubPub = configuration.GetValue<string>(nameof(PubnubPub));
        }

        public Task<BusConfigVM> GetConfig(string macAddress)
            => Task.FromResult(new BusConfigVM
            {
                BusId = Guid.NewGuid(),
                QueueName = QueueName,
                ServiceBusConnectionString = ServiceBusConnectionString
            });

        public Task<PubnubConfig> GetPubnub()
            => Task.FromResult(new PubnubConfig
            {
                PubKey = PubnubPub,
                SubKey = PubnubSub
            });
    }
}
