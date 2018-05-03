using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.WindowsAzure.ServiceRuntime;
using Newtonsoft.Json;
using Orleans;
using Orleans.Streams;
using Orleans.Runtime.Configuration;
using Orleans.Hosting;
using Orleans.Providers.Streams.AzureQueue;
using Orleans.Configuration;

using TuRuta.Common;
using TuRuta.Common.Device;
using TuRuta.Orleans.Interfaces;
using TuRuta.Common.Logger;

namespace TuRuta.Ingestor
{
    public class WorkerRole : RoleEntryPoint
    {
        private QueueClient QueueClient;
        private IStreamProvider streamProvider;
        private ManualResetEvent CompletedEvent = new ManualResetEvent(false);
        private IClusterClient client;

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

        private IClientBuilder GetClientBuilder()
        {
            var deploymentId = RoleEnvironment.DeploymentId.Replace("(", "-").Replace(")", "-");
            var isDevelopment = bool.Parse(RoleEnvironment.GetConfigurationSettingValue("IsDevelopment"));
            var connectionString = RoleEnvironment.GetConfigurationSettingValue("DataConnectionString");
            
            var builder = new ClientBuilder()
                .UseAzureStorageClustering(config => config.ConnectionString = connectionString)
                .Configure<ClusterOptions>(cluster => {
                    cluster.ClusterId = Constants.ClusterId;
                    cluster.ServiceId = "DaBus";
                })
                .ConfigureApplicationParts(
                    parts => parts.AddApplicationPart(typeof(IBusGrain).Assembly))
                .ConfigureLogging(logging => logging.AddAllTraceLoggers());

            if (isDevelopment)
            {
                builder.AddSimpleMessageStreamProvider("StreamProvider");
            }
            else
            {
                builder.AddSimpleMessageStreamProvider("StreamProvider");
            }

            return builder;
        } 

        private async Task RunAsync()
        {
            client = GetClientBuilder().Build();
            await client.Connect(async ex =>
            {
                Trace.TraceInformation($"{ex.Message}");

                await Task.Delay(TimeSpan.FromSeconds(5));
                return true;
            });

            Trace.TraceInformation("Orleans is initialized");

            var connectionString = RoleEnvironment.GetConfigurationSettingValue("QueueConnectionString");
            var queueName = RoleEnvironment.GetConfigurationSettingValue("QueueName");

            int attemps = 0;
            while (true)
            {
                try
                {
                    streamProvider = client.GetStreamProvider("StreamProvider");
                    break;
                }
                catch(Exception ex)
                {
                    if(attemps > 2)
                    {
                        break;
                    }

                    attemps++;
                    Trace.TraceError($"Error: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }

            QueueClient = new QueueClient(connectionString, queueName);
            
            var messageOptions = new MessageHandlerOptions(OnError)
            {
                MaxConcurrentCalls = Environment.ProcessorCount,
                AutoComplete = false
            };

            QueueClient.RegisterMessageHandler(ProccessMessage, messageOptions);

            Trace.TraceInformation("Queue is initialized");
        }

        private Task OnError(ExceptionReceivedEventArgs args)
        {
            Trace.TraceError(args.Exception.Message);

            return Task.CompletedTask;
        }

        private async Task ProccessMessage(Message message, CancellationToken token)
        {
            RouteBusUpdate Deserialize(byte[] data)
            {
                var json = Encoding.UTF32.GetString(data);
                return JsonConvert.DeserializeObject<RouteBusUpdate>(json);
            }

            var stream = streamProvider.GetStream<RouteBusUpdate>(Guid.Parse(message.To), "Buses");
            await stream.OnNextAsync(Deserialize(message.Body));

            await QueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}
