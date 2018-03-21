using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TuRuta.Bot.Services;
using TuRuta.Bot.Services.Interfaces;

namespace TuRuta.Bot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var credentialProvider = new StaticCredentialProvider(
                Configuration.GetValue<string>("MicrosoftAppIdKey"), 
                Configuration.GetValue<string>("MicrosoftAppPasswordKey"));

            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddBotAuthentication(credentialProvider);

            services.AddSingleton(_ => Configuration);
            services.AddSingleton<ILuisService, LuisService>();
            services.AddSingleton<IRoutesService, RoutesServices>();
            services.AddSingleton<IDialogService, DialogService>();

            services.AddMvc(options => options.Filters.Add(typeof(TrustServiceUrlAttribute)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
