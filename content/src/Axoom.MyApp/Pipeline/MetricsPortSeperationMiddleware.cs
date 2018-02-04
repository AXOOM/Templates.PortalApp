using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Axoom.MyApp.Pipeline
{
    /// <summary>
    /// Split out metrics requests to port 5000.
    /// </summary>
    public class MetricsPortSeperationMiddleware
    {
        private readonly RequestDelegate _next;

        public MetricsPortSeperationMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            bool isMetricsPort = (context.Connection.LocalPort == 5000);
            bool isMetricsPath = context.Request.Path.StartsWithSegments("/metrics");

            if (isMetricsPort != isMetricsPath)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
