using System;
using Axoom.Extensions.Configuration.Yaml;
using Axoom.Extensions.Logging.Console;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Axoom.MyApp
{
    /// <summary>
    /// Startup class used by ASP.NET Core.
    /// </summary>
    [UsedImplicitly]
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Called by ASP.NET Core to set up an environment.
        /// </summary>
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddYamlFile("appsettings.yml", optional: false, reloadOnChange: true)
                .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Called by ASP.NET Core to register services.
        /// </summary>
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services) => services
            .AddRestApi()
            .AddLogging(builder => builder.AddConfiguration(Configuration.GetSection("Logging")))
            .AddOptions()
            //.Configure<MyOptions>(Configuration.GetSection("MyOptions"))
            //.AddTransient<IMyService, MyService>()
            //.AddSingleton<Worker>()
            ;

        /// <summary>
        /// Called by ASP.NET Core to configure services after they have been registered.
        /// </summary>
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            loggerFactory
                .AddAxoomConsole(Configuration.GetSection("Logging"))
                .CreateLogger<Startup>()
                .LogInformation("Starting My App");

            app.UseRestApi(env);
        }
    }
}