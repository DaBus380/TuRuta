using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuRuta.Common.ViewModels;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Services
{
    public class MockConfigService : IConfigService
    {
        private readonly string QueueName;
        private readonly string ServiceBusConnectionString;
        public MockConfigService(IConfiguration configuration)
        {
            QueueName = configuration.GetValue<string>("QueueName");
            ServiceBusConnectionString = configuration.GetValue<string>("QueueConnectionString");
        }

        public Task<BusConfigVM> GetConfig(string macAddress)
            => Task.FromResult(new BusConfigVM
            {
                BusId = Guid.NewGuid(),
                QueueName = QueueName,
                ServiceBusConnectionString = ServiceBusConnectionString
            });
    }
}
