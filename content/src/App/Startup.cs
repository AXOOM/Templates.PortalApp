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

        // Register services for DI
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddPrometheusServer(_configuration.GetSection("Metrics"))
                    .AddSecurity(_configuration.GetSection("Authentication"))
                    .AddWeb();

            string dbConnectionString = _configuration.GetConnectionString("Database");
            services.AddDbContext<DbContext>(options =>
            {
                if (dbConnectionString.StartsWith("Data Source=")) options.UseSqlite(dbConnectionString);
                else options.UseNpgsql(dbConnectionString);
            });

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

        // Tasks that need to run before serving HTTP requests
        public static void Init(IServiceProvider provider)
        {
            // TODO: Replace .EnsureCreated() with .Migrate() once you start using EF Migrations
            provider.GetRequiredService<DbContext>().Database.EnsureCreated();
        }
    }
}
