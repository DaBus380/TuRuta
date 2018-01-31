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

namespace TuRuta.Orleans
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("TuRuta.Orleans is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Establecer el número máximo de conexiones simultáneas
            ServicePointManager.DefaultConnectionLimit = 12;

            // Para obtener información sobre cómo administrar los cambios de configuración
            // consulte el tema de MSDN en https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("TuRuta.Orleans has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("TuRuta.Orleans is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("TuRuta.Orleans has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Reemplazar lo siguiente por su propia lógica.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
