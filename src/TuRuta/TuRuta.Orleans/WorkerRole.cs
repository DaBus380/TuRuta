using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Orleans.Runtime.Configuration;
using Orleans.Hosting;
using Orleans;
using Orleans.Providers.Streams.AzureQueue;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;

using TuRuta.Orleans.Grains;
using TuRuta.Common.Logger;
using TuRuta.Orleans.Grains.Services.Interfaces;
using TuRuta.Orleans.Grains.Services;
using Orleans.Storage;

namespace TuRuta.Orleans
{
    public class WorkerRole : RoleEntryPoint
    {
        private ISiloHost silo;
        private ManualResetEvent resetEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("TuRuta.Orleans is running");
            try
            {
                RunAsync().Wait();

                resetEvent.WaitOne();
            }
            finally
            {
                resetEvent.Set();
            }
        }

        public override bool OnStart()
            => base.OnStart();

        public override void OnStop()
        {
            Trace.TraceInformation("TuRuta.Orleans is stopping");

            silo.StopAsync().Wait();

            base.OnStop();

            Trace.TraceInformation("TuRuta.Orleans has stopped");
        }

        private ISiloHostBuilder GetHostBuilder()
        {
            var proxyPort = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["OrleansProxyEndpoint"].IPEndpoint.Port;
            var siloEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["OrleansSiloEndpoint"].IPEndpoint;
            var deploymentId = RoleEnvironment.DeploymentId.Replace("(", "-").Replace(")", "-");
            var isDevelopment = bool.Parse(RoleEnvironment.GetConfigurationSettingValue("IsDevelopment"));
            var connectionString = RoleEnvironment.GetConfigurationSettingValue("DataConnectionString");

            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "DaBus";
                    options.ServiceId = "DaBus";
                })
                .ConfigureEndpoints(siloEndpoint.Address, siloEndpoint.Port, proxyPort)
                .ConfigureLogging(logging => logging.AddAllTraceLoggers())
                .UseServiceProviderFactory(services =>
                {
                    services.AddSingleton<IDistanceCalculator, HavesineDistanceCalculator>();
                    services.AddSingleton<IConfigClient, ConfigClient>();

                    return services.BuildServiceProvider();
                })
                .ConfigureApplicationParts(
                    parts => parts.AddApplicationPart(typeof(BusGrain).Assembly).WithReferences())
                .UseAzureStorageClustering(options => options.ConnectionString = connectionString);

            if (isDevelopment)
            {
                builder
                    .UseInMemoryReminderService()
                    .AddMemoryGrainStorage("AzureTableStore")
                    .AddMemoryGrainStorage("PubSubStore")
                    .AddSimpleMessageStreamProvider("StreamProvider");
            }
            else
            {
                builder
                    .UseAzureTableReminderService(connectionString)
                    .AddAzureQueueStreams<AzureQueueDataAdapterV2>("StreamProvider", configuratior =>
                    {
                        configuratior.Configure(options =>
                        {
                            options.ConnectionString = connectionString;
                        });
                    })
                    .AddAzureTableGrainStorage("AzureTableStore", options => options.UseJson = true)
                    .AddAzureTableGrainStorage("PubSubStore", options => options.UseJson = true);
            }

            return builder;
        }

        private async Task RunAsync()
        {
            silo = GetHostBuilder().Build();
            await silo.StartAsync();
        }
    }
}
