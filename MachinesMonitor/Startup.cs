using System;
using System.Threading.Tasks;
using Data.Infrastructure.MSSQL;
using Domain.Services;
using MachinesMonitor.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MachinesMonitor
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            ContentRootPath = env.ContentRootPath;
            Configuration = configuration;
        }
        public string ContentRootPath { get; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSqlStorage(Configuration.GetValue<string>("Db:ConnectionString"));
            services.AddMvc();
            services.AddVehicleMonitoringService(Configuration.GetValue<string>("VehicleMonitoring:PingTimeout")); //No worth to await here, just need async call
            services.AddSingleton<IHostedService>( //Use IHostedService as idiomatic approach for running long background actions
                s =>
                {
                    var actions = s.GetService<Tuple<Action, Action>>();
                    return new PeriodicBackgroundRunner(actions.Item1, actions.Item2);
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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
                    template: "{controller=VehiclesStatus}/{action=Monitoring}/{id?}"
                    );
            });

            var backgroundTaskRunner = serviceProvider.GetService<IHostedService>();
        }
    }
}
