using System;
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
    /// Startup class used by ASP.NET Core.
    /// </summary>
    [UsedImplicitly]
    public class Startup : IStartup
    {
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Called by ASP.NET Core to set up an environment.
        /// </summary>
        public Startup(IHostingEnvironment env = null)
        {
            var builder = new ConfigurationBuilder();
            if (env != null)
                builder.SetBasePath(env.ContentRootPath);
            builder.AddYamlFile("appsettings.yml", optional: false, reloadOnChange: true);
            if (env != null)
                builder.AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// Called by ASP.NET Core to register services.
        /// </summary>
        public IServiceProvider ConfigureServices(IServiceCollection services)
            => services.AddInfrastructure(Configuration)
                       .AddDbContext<DbContext>(options => options.UseSqlite(Configuration.GetSection("Database").GetValue<string>("ConnectionString")))
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
                    scope.ServiceProvider.GetRequiredService<DbContext>().Database.EnsureCreated();
            });
        }
    }
}
