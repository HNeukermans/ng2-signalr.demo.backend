using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Owin;
using Microsoft.AspNet.SignalR;
using ng2SignalR.hub.Hubs;
using Autofac;
using Ng2Aa_demo.Extensions.Microsoft.AspNetCore.Builder;

namespace ng2SignalR.hub
{
    public class Startup
    {
        public IHostingEnvironment HostingEnvironment { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddMvc();
        }

       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //UseAutofac(app);

            app.UseDeveloperExceptionPage();

            app.UseAppBuilder(appBuilder =>
            {
                appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            });
                       
            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());

            app.UseStaticFiles();

            app.UseAppBuilder(appBuilder =>
            {
                //create SignalR JavaScript Library  and expose it over signalr/hubs url
                var hubConfiguration = new HubConfiguration();
                hubConfiguration.EnableDetailedErrors = true;
                appBuilder.MapSignalR(hubConfiguration);
            });

            app.UseMvc();
        }

       

    }

   
}
