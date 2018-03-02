using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Providers;
using Orleans.Providers.Streams.AzureQueue;
using TuRuta.Orleans.Interfaces;
using TuRuta.Web.Services;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var isRunning = Configuration.GetValue<bool>("ORLEANS_RUNNING");

            if (isRunning)
            {
                int attemps = 0;
                while (true)
                {
                    try
                    {
                        var client = GetClientBuilder().Build();
                        client.Connect().Wait();

                        services.AddSingleton(client);
                        break;
                    }
                    catch (Exception ex)
                    {
                        attemps++;
                        if(attemps > 3)
                        {
                            throw ex;
                        }

                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }

                services.AddSingleton<IRoutesService, RoutesService>();
            }
            
            services.AddSingleton<IConfigService, MockConfigService>();
            
            services
                .AddMvc(options => options.RespectBrowserAcceptHeader = true)
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

        private IClientBuilder GetClientBuilder()
        {
            var connectionString = Configuration.GetValue<string>("DataConnectionString");

            var builder = new ClientBuilder()
                .ConfigureCluster(cluster => cluster.ClusterId = "DaBus")
                .UseAzureStorageClustering(config => config.ConnectionString = connectionString)
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureApplicationParts(
                    parts => parts.AddApplicationPart(typeof(IBusGrain).Assembly));

            if (Env.IsDevelopment())
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
