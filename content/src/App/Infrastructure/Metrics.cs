using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nexogen.Libraries.Metrics;
using Nexogen.Libraries.Metrics.Prometheus;
using Nexogen.Libraries.Metrics.Prometheus.Standalone;

namespace MyVendor.MyApp.Infrastructure
{
    public static class Metrics
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services, IConfiguration configuration)
        {
            var metrics = new PrometheusMetrics();
            return services.Configure<MetricsOptions>(configuration.GetSection("Metrics"))
                           .AddSingleton<IMetrics>(metrics)
                           .AddSingleton<IExposable>(metrics);
        }

        public static IDisposable ExposeMetrics(this IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<MetricsOptions>>().Value;
            return provider.GetRequiredService<IExposable>().Server().Port(options.Port).Start();
        }
    }

    public class MetricsOptions
    {
        public int Port { get; set; }
    }
}
