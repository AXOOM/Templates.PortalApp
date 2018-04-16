using System;
using Microsoft.Extensions.DependencyInjection;
using Nexogen.Libraries.Metrics;
using Nexogen.Libraries.Metrics.Prometheus;
using Nexogen.Libraries.Metrics.Prometheus.Standalone;

namespace Axoom.MyApp.Infrastructure
{
    public static class Metrics
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            var metrics = new PrometheusMetrics();
            return services
                .AddSingleton<IMetrics>(metrics)
                .AddSingleton<IExposable>(metrics);
        }

        public static IDisposable ExposeMetrics(this IServiceProvider provider, int port)
            => provider.GetRequiredService<IExposable>().Server().Port(port).Start();
    }
}
