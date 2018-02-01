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
using Orleans.Providers;
using Orleans.Hosting;
using Orleans.Runtime.Host;
using Orleans;
using Orleans.AzureUtils;
using Orleans.Extensions;
using Orleans.Runtime.Configuration;
using Microsoft.Extensions.Logging;

namespace TuRuta.Orleans
{
    public class WorkerRole : RoleEntryPoint
    {
        private ISiloHost silo;
        public override void Run()
        {
            Trace.TraceInformation("TuRuta.Orleans is running");

            RunAsync().Wait();
        }

        public override bool OnStart()
        {

            Trace.TraceInformation("TuRuta.Orleans has been started");

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("TuRuta.Orleans is stopping");

            silo.StopAsync().Wait();

            base.OnStop();

            Trace.TraceInformation("TuRuta.Orleans has stopped");
        }

        private async Task RunAsync()
        {
            var config = AzureSilo.DefaultConfiguration();
            config.AddAzureTableStorageProvider("Storage");
            config.AddAzureQueueStreamProviderV2("Queues");

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .ConfigureLogging(logging => logging.AddConsole());

            silo = builder.Build();
            await silo.StartAsync();
        }
    }
}
