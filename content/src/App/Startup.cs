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
    [UsedImplicitly]
    public class Startup : IStartup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Register services
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddPrometheusServer(_configuration.GetSection("Metrics"))
                    .AddSecurity(_configuration.GetSection("Authentication"))
                    .AddWeb();

            services.AddDbContext<DbContext>(options => options
                // TODO: Replace SQLite with external database for scalability
               .UseSqlite(_configuration.GetConnectionString("Database")));

            services.AddHealthChecks()
                    .AddDbContextCheck<DbContext>();

            services.AddContacts();

            return services.BuildServiceProvider();
        }

        // Configure HTTP request pipeline
        public void Configure(IApplicationBuilder app)
            => app.UseHealthChecks("/health")
                  .UseSecurity()
                  .UseWeb();

        // Run startup tasks
        public static void Init(IServiceProvider provider)
        {
            // TODO: Replace .EnsureCreated() with .Migrate() once you start using EF Migrations
            provider.GetRequiredService<DbContext>().Database.EnsureCreated();
        }
    }
}
