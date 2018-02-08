using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime.Host;
using Orleans.Runtime.Configuration;
using Orleans.Streams;
using TuRuta.Common.Device;

namespace TuRuta.Ingestor
{
    public class WorkerRole : RoleEntryPoint
    {
        private QueueClient QueueClient;
        private int attempsBeforeFailing = 2;
        private IStreamProvider streamProvider;
        private ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            RunAsync().Wait();

            Trace.TraceInformation("TuRuta.Ingestor is running");

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            bool result = base.OnStart();

            Trace.TraceInformation("TuRuta.Ingestor has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("TuRuta.Ingestor is stopping");

            QueueClient.CloseAsync().Wait();
            CompletedEvent.Set();

            base.OnStop();

            Trace.TraceInformation("TuRuta.Ingestor has stopped");
        }

        private async Task RunAsync()
        {
            var deploymentId = RoleEnvironment.DeploymentId.Replace("(", "-").Replace(")", "");
            var config = AzureClient.DefaultConfiguration();
            config.AddAzureQueueStreamProviderV2("StreamProvider", deploymentId: deploymentId);

            int attemps = 0;
            while (true)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    AzureClient.Initialize(config);
                    Trace.TraceInformation("Orleans is initialized");
                    break;
                }
                catch (Exception)
                {
                    attemps++;
                    Trace.TraceWarning("Orleans is not ready");
                    if (attemps > attempsBeforeFailing)
                    {
                        throw;
                    }
                }
            }

            var connectionString = RoleEnvironment.GetConfigurationSettingValue("QueueConnectionString");
            var queueName = RoleEnvironment.GetConfigurationSettingValue("QueueName");

            while (true)
            {
                if (GrainClient.GetStreamProviders().Count() != 0)
                {
                    streamProvider = GrainClient.GetStreamProvider("StreamProvider");
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }

            QueueClient = new QueueClient(connectionString, queueName);
            
            var messageOptions = new MessageHandlerOptions(OnError)
            {
                MaxConcurrentCalls = 5,
                AutoComplete = false
            };

            QueueClient.RegisterMessageHandler(ProccessMessage, messageOptions);

            Trace.TraceInformation("Queue is initialized");
        }

        private Task OnError(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine(args.Exception.Message);

            return Task.CompletedTask;
        }

        private async Task ProccessMessage(Message message, CancellationToken token)
        {
            PositionUpdate Deserialize(byte[] data)
            {
                var json = Encoding.UTF32.GetString(data);
                return JsonConvert.DeserializeObject<PositionUpdate>(json);
            }

            var stream = streamProvider.GetStream<PositionUpdate>(Guid.Parse(message.To), "Buses");
            await stream.OnNextAsync(Deserialize(message.Body));

            await QueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}
