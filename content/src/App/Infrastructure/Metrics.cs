using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nexogen.Libraries.Metrics;
using Nexogen.Libraries.Metrics.Prometheus;

namespace MyVendor.MyApp.Infrastructure
{
    public static class Metrics
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services, IConfiguration configuration)
        {
            var metrics = new PrometheusMetrics();
            return services.Configure<MetricsOptions>(configuration.GetSection("Metrics"))
                           .AddSingleton<IMetrics>(metrics)
                           .AddSingleton<IExposable>(metrics)
                           .AddSingleton<MetricsServer>();
        }

        public static void ExposeMetrics(this IServiceProvider provider)
            => provider.GetRequiredService<MetricsServer>();
    }

    public class MetricsOptions
    {
        /// <summary>
        /// The port number to expose Prometheus metrics on.
        /// </summary>
        public int Port { get; set; }
    }

    /// <summary>
    /// Exposes Prometheus metrics on a seperate port.
    /// </summary>
    public class MetricsServer : IDisposable
    {
        private readonly HttpListener _listener;

        /// <summary>
        /// Starts exposing metrics.
        /// </summary>
        public MetricsServer(IExposable metrics, IOptions<MetricsOptions> options, ILogger<MetricsServer> logger)
        {
            _listener = new HttpListener {Prefixes = {$"http://*:{options.Value.Port}/"}};
            _listener.Start();
            _listener.BeginGetContext(ListenerCallback, _listener);

            async void ListenerCallback(IAsyncResult result)
            {
                try
                {
                    var context = _listener.EndGetContext(result);
                    if (context.Request.HttpMethod == "GET")
                    {
                        context.Response.StatusCode = 200;
                        context.Response.Headers.Add("Content-Type", "text/plain");
                        await metrics.Expose(context.Response.OutputStream, ExposeOptions.Default);
                    }
                    else context.Response.StatusCode = 405; // Method not allowed

                    context.Response.Close();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while providing metrics");
                }
                finally
                {
                    _listener.BeginGetContext(ListenerCallback, _listener);
                }
            }
        }

        /// <summary>
        /// Stops exposing metrics.
        /// </summary>
        public void Dispose() => _listener.Abort();
    }
}
