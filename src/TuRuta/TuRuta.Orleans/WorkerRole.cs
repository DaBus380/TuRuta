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
                .ConfigureLogging(logging =>
                {
                    logging.AddProvider(new TraceLoggerProvider((_, level) => level == Microsoft.Extensions.Logging.LogLevel.Trace));
                })
                .ConfigureApplicationParts(
                parts => parts.AddApplicationPart(typeof(BusGrain).Assembly).WithReferences());

            var siloBuilted = builder.Build();
            await siloBuilted.StartAsync();
        }
    }

    class TraceLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, Microsoft.Extensions.Logging.LogLevel, bool> _filter;

        public TraceLoggerProvider(
            Func<string, Microsoft.Extensions.Logging.LogLevel, bool>  filter)
        {
            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName) => new TraceLogger(categoryName, _filter);

        public void Dispose() { }
    }

    class TraceLogger : ILogger
    {
        private string _categoryName;
        private Func<string, Microsoft.Extensions.Logging.LogLevel, bool> _filter;

        public TraceLogger(string categoryName, Func<string, Microsoft.Extensions.Logging.LogLevel, bool> filter)
        {
            _filter = filter;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
            => (_filter == null || _filter(_categoryName, logLevel)); 

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    Trace.TraceInformation(message);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    Trace.TraceError(message);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    Trace.TraceInformation(message);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    Trace.TraceWarning(message);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    Trace.TraceError(message);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    Trace.TraceError(message);
                    break;
                default:
                    break;
            }

            if (!IsEnabled(logLevel))
            {
                return;
            }

            if(formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"{logLevel}: {message}";

            if(exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception.ToString();
            }

            Trace.WriteLine(message);
        }
    }
}
