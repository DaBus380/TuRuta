using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using TuRuta.Web.Extensions;
using TuRuta.Web.Services.Mocks;
using TuRuta.Web.Extensions.AzureAD;
using TuRuta.Web.Services.Interfaces;

namespace TuRuta.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var isRunning = Configuration.GetValue<bool>("ORLEANS_RUNNING");

            services.AddAuthentication(options
                => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
            .AddAzureAdBearer(options => Configuration.Bind("AzureAD", options));

            if (isRunning)
            {
                services.AddOrleans(Configuration, Env);
            }
            else
            {
                services.AddSingleton<IConfigService, MockConfigService>();
                services.AddSingleton<IRoutesService, MockRoutesService>();
                services.AddSingleton<IBusService, MockBusService>();
                services.AddSingleton<IStopService, MockStopService>();
            }

            if (Env.IsDevelopment())
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                    {
                        Version = "v1",
                        Title = "TuRuta API",
                        Description = "You know what to do",
                        TermsOfService = "This web API is private, do not use unless you are authorize to use it."
                    });
                });
            }
            
            services
                .AddMvc(options => options.RespectBrowserAcceptHeader = true)
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TuRuta API v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

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
    }
}
