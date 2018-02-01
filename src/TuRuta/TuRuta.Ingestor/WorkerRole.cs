using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.ServiceBus;
using Orleans;
using Microsoft.ServiceBus.Messaging;
using Orleans.Hosting;
using Orleans.Runtime.Host;
using Orleans.Runtime.Configuration;
using Microsoft.Extensions.Logging;

namespace TuRuta.Ingestor
{
    public class WorkerRole : RoleEntryPoint
    {
        private IClusterClient client;
        private QueueClient queue;
        private string QueueName = "";

        public override void Run()
        {
            Trace.TraceInformation("TuRuta.Ingestor is running");
            
            this.RunAsync().Wait();
        }

        public override bool OnStart()
        {
            bool result = base.OnStart();
            var config = AzureClient.DefaultConfiguration();
            var builder = new ClientBuilder()
                .UseConfiguration(config)
                .ConfigureLogging(logging => logging.AddConsole());

            client = builder.Build();

            var connectionString = RoleEnvironment.GetConfigurationSettingValue("QueueConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            queue = QueueClient.CreateFromConnectionString(connectionString, QueueName, ReceiveMode.PeekLock);

            Trace.TraceInformation("TuRuta.Ingestor has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("TuRuta.Ingestor is stopping");

            client.Dispose();
            base.OnStop();

            Trace.TraceInformation("TuRuta.Ingestor has stopped");
        }

        private async Task RunAsync()
        {
            await client.Connect();
            var queueProvider = client.GetStreamProvider("Queues");
            
            queue.OnMessageAsync(async message =>
            {
                var id = Guid.Parse(message.To);
                var stream = queueProvider.GetStream<object>(id, "PositionUpdate");
                await stream.OnNextAsync(new { });
            });
        }
    }
}
