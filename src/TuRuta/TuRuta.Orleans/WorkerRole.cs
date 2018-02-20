using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Orleans.Runtime.Host;
using Orleans.Providers;
using Orleans.Runtime.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Text;

using TuRuta.Orleans.Grains;
using TuRuta.Common.Logger;

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

        private async Task RunAsync()
        {

            var deploymentId = RoleEnvironment.DeploymentId.Replace("(", "-").Replace(")", "");
            var connectionString = RoleEnvironment.GetConfigurationSettingValue("DataConnectionString");
            var proxyEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["OrleansProxyEndpoint"].IPEndpoint;
            var siloEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["OrleansSiloEndpoint"].IPEndpoint;

            var config = AzureSilo.DefaultConfiguration();
            config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.AzureTable;
            //config.AddSimpleMessageStreamProvider("StreamProvider");
            //config.AddMemoryStorageProvider("AzureTableStore");
            config.AddMemoryStorageProvider("PubSubStore");
            config.Defaults.HostNameOrIPAddress = siloEndpoint.Address.ToString();
            config.Defaults.Port = siloEndpoint.Port;
            config.Defaults.ProxyGatewayEndpoint = proxyEndpoint;
            
            config.AddAzureQueueStreamProviderV2("StreamProvider", connectionString: connectionString, clusterId: deploymentId);
            config.AddAzureTableStorageProvider(connectionString: connectionString);
            

            config.Globals.ClusterId = deploymentId;

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .ConfigureLogging(logging => logging.AddAllTraceLoggers())
                .ConfigureApplicationParts(
                parts => parts.AddApplicationPart(typeof(BusGrain).Assembly).WithReferences());

            silo = builder.Build();
            await silo.StartAsync();
        }
    }
}
