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
using Orleans.Runtime.Host;
using Orleans.Providers;
using Orleans.Runtime.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using TuRuta.Orleans.Grains;
using Orleans;

namespace TuRuta.Orleans
{
    public class WorkerRole : RoleEntryPoint
    {
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

            /*
            silo = new AzureSilo();
            var isGood = silo.Start(config);
            if (isGood)
            {
                silo.Run();
            }
            */
        }

        public override bool OnStart()
            => base.OnStart();

        public override void OnStop()
        {
            Trace.TraceInformation("TuRuta.Orleans is stopping");

            base.OnStop();

            Trace.TraceInformation("TuRuta.Orleans has stopped");
        }

        private async Task RunAsync()
        {

            var deploymentId = RoleEnvironment.DeploymentId.Replace("(", "-").Replace(")", "");
            var connectionString = RoleEnvironment.GetConfigurationSettingValue("DataConnectionString");
            var proxyPort = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["OrleansProxyEndpoint"].IPEndpoint.Port;
            var endpointPort = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["OrleansSiloEndpoint"].IPEndpoint.Port;

            var config = AzureSilo.DefaultConfiguration();
            config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.AzureTable;
            //config.AddSimpleMessageStreamProvider("StreamProvider");
            //config.AddMemoryStorageProvider("AzureTableStore");
            config.AddMemoryStorageProvider("PubSubStore");
            config.Defaults.Port = endpointPort;
            config.Defaults.ProxyGatewayEndpoint = new IPEndPoint(
                config.Defaults.Endpoint.Address, proxyPort);
            
            config.AddAzureQueueStreamProviderV2("StreamProvider", connectionString: connectionString);
            config.AddAzureTableStorageProvider(connectionString: connectionString);
            

            config.Globals.ClusterId = deploymentId;

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureApplicationParts(
                parts => parts.AddApplicationPart(typeof(BusGrain).Assembly).WithReferences());

            var siloBuilted = builder.Build();
            await siloBuilted.StartAsync();
        }
    }
}
