using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers;
using Orleans.Providers.Streams.AzureQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TuRuta.Common;
using TuRuta.Web.Services;
using TuRuta.Orleans.Interfaces;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web.Extensions
{
    public static class OrleansClientExtensions
    {
        public static IServiceCollection AddOrleans(
            this IServiceCollection services, 
            IConfiguration configuration, 
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var servicesBuilted = services.BuildServiceProvider();
            var logger = servicesBuilted.GetService<ILoggerFactory>().CreateLogger("Debug");

            var client = GetClientBuilder(configuration, env).Build();
            client.Connect(async ex => 
            {
                logger.LogInformation($"{ex.Message}");

                await Task.Delay(TimeSpan.FromSeconds(5));
                return true;
            }).Wait();

            services.AddSingleton(client);

            services.AddSingleton<IRoutesService, RoutesService>();
            services.AddSingleton<IBusService, BusService>();
            services.AddSingleton<IConfigService, ConfigService>();
            services.AddSingleton<IStopService, StopsService>();

            return services;
        }

        private static IClientBuilder GetClientBuilder(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var connectionString = configuration.GetValue<string>("DataConnectionString");

            var builder = new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = Constants.ClusterId;
                    options.ServiceId = "DaBus";
                })
                .UseAzureStorageClustering(config => config.ConnectionString = connectionString)
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureApplicationParts(
                    parts => parts.AddApplicationPart(typeof(IBusGrain).Assembly));

            if (env.IsDevelopment())
            {
                builder.AddMemoryStreams<DefaultMemoryMessageBodySerializer>("StreamProvider");
            }
            else
            {
                builder.AddAzureQueueStreams<AzureQueueDataAdapterV2>("StreamProvider", configurator =>
                {
                    configurator.Configure(options =>
                    {
                        options.ConnectionString = connectionString;
                    });
                });
            }

            return builder;
        }
    }
}
