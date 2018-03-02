using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Providers;
using Orleans.Providers.Streams.AzureQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuRuta.Orleans.Interfaces;
using TuRuta.Web.Services;
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
            int attemps = 0;
            while (true)
            {
                try
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    var client = GetClientBuilder(configuration, env).Build();
                    client.Connect().Wait();

                    services.AddSingleton(client);
                    break;
                }
                catch (Exception ex)
                {
                    attemps++;
                    if (attemps > 3)
                    {
                        throw ex;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }

            services.AddSingleton<IRoutesService, RoutesService>();

            return services;
        }

        private static IClientBuilder GetClientBuilder(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var connectionString = configuration.GetValue<string>("DataConnectionString");

            var builder = new ClientBuilder()
                .ConfigureCluster(cluster => cluster.ClusterId = "DaBus")
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
                builder.AddAzureQueueStreams<AzureQueueDataAdapterV2>("StreamProvider");
            }

            return builder;
        }
    }
}
