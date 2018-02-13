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
using TuRuta.Orleans.Grains;
using Orleans;
using Microsoft.Extensions.Logging;

namespace TuRuta.Orleans
{
    public class WorkerRole : RoleEntryPoint
    {
        private ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("TuRuta.Orleans is running");
            
            var config = AzureSilo.DefaultConfiguration();
            config.AddAzureTableStorageProvider();
            config.AddAzureQueueStreamProviderV2("StreamProvider");
            config.AddMemoryStorageProvider("PubSubStore");
            config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.AzureTable;

            config.Globals.ClusterId = RoleEnvironment.DeploymentId.Replace("(", "-").Replace(")", "");

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(BusGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            Start(host).GetAwaiter().GetResult();

            CompletedEvent.WaitOne();
        }

        private Task Start(ISiloHost host)
            => host.StartAsync();

        public override bool OnStart()
            => base.OnStart();

        public override void OnStop()
        {
            Trace.TraceInformation("TuRuta.Orleans is stopping");

            base.OnStop();

            Trace.TraceInformation("TuRuta.Orleans has stopped");
        }
    }
}
