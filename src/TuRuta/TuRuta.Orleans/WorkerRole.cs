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

namespace TuRuta.Orleans
{
    public class WorkerRole : RoleEntryPoint
    {
        private AzureSilo silo;

        public override void Run()
        {
            Trace.TraceInformation("TuRuta.Orleans is running");

            var deploymentId = RoleEnvironment.DeploymentId.Replace("(", "-").Replace(")", "");

            var config = AzureSilo.DefaultConfiguration();
            config.AddAzureTableStorageProvider();
            config.AddAzureQueueStreamProviderV2("StreamProvider", deploymentId: deploymentId);

            silo = new AzureSilo();
            var isGood = silo.Start(config);
            if (isGood)
            {
                silo.Run();
            }
        }

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
