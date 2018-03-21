using System;
using Microsoft.Extensions.DependencyInjection;
using Nexogen.Libraries.Metrics;
using Nexogen.Libraries.Metrics.Extensions;
using Nexogen.Libraries.Metrics.Prometheus;
using Nexogen.Libraries.Metrics.Prometheus.Standalone;

namespace Axoom.MyApp
{
    /// <summary>
    /// Used to report service-specific metrics.
    /// </summary>
    public interface IMyServiceMetrics
    {
        /// <summary>
        /// Counts occurances of an event.
        /// </summary>
        void MyEvent();

        /// <summary>
        /// Starts a timer for the duration of a job. Place inside a using statement.
        /// </summary>
        IDisposable TimeJob();
    }

    /// <summary>
    /// Used to report service-specific metrics.
    /// </summary>
    public class MyServiceMetrics : IMyServiceMetrics
    {
        private readonly ICounter _myEvent;
        private readonly IHistogram _myJob;

        public MyServiceMetrics(IMetrics metrics)
        {
            _myEvent = metrics.Counter()
                .Name("my_event")
                .Help("Occuranes of an event")
                .Register();
            _myJob = metrics.Histogram()
                .Name("my_job")
                .Help("Avergae duration of a job")
                .Register();
        }

        /// <inheritdoc/>
        public void MyEvent() => _myEvent.Increment();

        /// <inheritdoc/>
        public IDisposable TimeJob() => _myJob.Timer();
    }

    public static class MetricsExtensions
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            var metrics = new PrometheusMetrics();
            return services
                .AddSingleton<IMetrics>(metrics)
                .AddSingleton<IExposable>(metrics)
                .AddSingleton<IMyServiceMetrics, MyServiceMetrics>();
        }

        public static IDisposable ExposeMetrics(this IServiceProvider provider, int port)
            => provider.GetRequiredService<IExposable>().Server().Port(port).Start();
    }
}
