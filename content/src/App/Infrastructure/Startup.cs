using System;
using Axoom.Extensions.Logging.Console;
using Axoom.Extensions.Prometheus.Standalone;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyVendor.MyApp.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
            => services.AddSingleton(configuration)
                       .AddOptions()
                       .AddLogging(builder => builder.AddConfiguration(configuration.GetSection("Logging"))
                                                     .AddAxoomConsole(configuration.GetSection("Logging"))
                                                     .AddExceptionDemystifyer())
                       .AddPrometheusServer(configuration.GetSection("Metrics"))
                       .AddPolicies(configuration.GetSection("Policies"))
                       .AddWeb(configuration);

        public static IServiceProvider UseInfrastructure(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;

            provider.UsePrometheusServer();

            app.UseWeb();

            return provider;
        }
    }
}
