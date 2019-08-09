using System;
using Axoom.Extensions.Prometheus.Standalone;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyVendor.MyApp.Contacts;
using MyVendor.MyApp.Infrastructure;

namespace MyVendor.MyApp
{
    /// <summary>
    /// Configures dependency injection.
    /// </summary>
    [UsedImplicitly]
    public class Startup : IStartup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Called by ASP.NET Core to set up an environment.
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Called by ASP.NET Core to register services.
        /// </summary>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddPrometheusServer(Configuration.GetSection("Metrics"))
                    .AddSecurity(Configuration.GetSection("Authentication"))
                    .AddWeb();

            services.AddDbContext<DbContext>(options => options
               .UseSqlite(Configuration.GetConnectionString("Database")));

            services.AddHealthChecks()
                    .AddDbContextCheck<DbContext>();

            services.AddContacts();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Called by ASP.NET Core to configure services after they have been registered.
        /// </summary>
        public void Configure(IApplicationBuilder app)
            => app.UseHealthChecks("/health")
                  .UseSecurity()
                  .UseWeb();

        /// <summary>
        /// Called after services have been configured but before web hosting started.
        /// </summary>
        public static void Init(IServiceProvider provider)
        {
            // Replace .EnsureCreated() with .Migrate() once you have generated an EF Migration
            provider.GetRequiredService<DbContext>().Database.EnsureCreated();
        }
    }
}
