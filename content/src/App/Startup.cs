using System;
using Axoom.MyApp.Contacts;
using Axoom.MyApp.Infrastructure;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Axoom.MyApp
{
    /// <summary>
    /// Startup class used by ASP.NET Core.
    /// </summary>
    [UsedImplicitly]
    public class Startup : IStartup
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
            => services.AddInfrastructure(Configuration)
                       .AddDbContext<MyAppDbContext>(options => options.UseSqlite(Configuration.GetSection("Database").GetValue<string>("ConnectionString")))
                       .AddContacts()
                       .BuildServiceProvider();

        /// <summary>
        /// Called by ASP.NET Core to configure services after they have been registered.
        /// </summary>
        public void Configure(IApplicationBuilder app)
        {
            var provider = app.UseInfrastructure();

            // Since SQLite is an in-process database resiliency against connectivity problems at startup is unnecessary.
            // It is implemented here anyway as a sample in case you decide to use an external database such as PostgreSQL.
            provider.GetRequiredService<Policies>().Startup(() =>
            {
                using (var scope = provider.CreateScope())
                    // Replace .EnsureCreated() with .Migrate() once you have generated an EF Migration
                    scope.ServiceProvider.GetRequiredService<MyAppDbContext>().Database.EnsureCreated();
            });
        }
    }
}
