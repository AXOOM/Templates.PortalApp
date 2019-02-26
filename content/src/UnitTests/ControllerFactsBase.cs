using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyVendor.MyApp.Infrastructure;
using Xunit.Abstractions;

namespace MyVendor.MyApp
{
    /// <summary>
    /// Sets up an in-memory version of the ASP.NET MVC stack for decoupled testing of controllers.
    /// </summary>
    public abstract class ControllerFactsBase : IDisposable
    {
        private readonly TestServer _server;

        protected ControllerFactsBase(ITestOutputHelper output)
        {
            _server = new TestServer(
                new WebHostBuilder()
                   .ConfigureLogging(builder => builder.AddXUnit(output))
                   .ConfigureServices((context, services) => services.AddWeb())
                   .ConfigureServices(ConfigureService)
                   .Configure(builder => builder.UseWeb()));

            HttpClient = _server.CreateClient();
        }

        /// <summary>
        /// Registers dependencies for controllers.
        /// </summary>
        protected abstract void ConfigureService(IServiceCollection services);

        /// <summary>
        /// A client configured for in-memory communication with ASP.NET MVC controllers.
        /// </summary>
        protected readonly HttpClient HttpClient;

        public virtual void Dispose()
        {
            HttpClient.Dispose();
            _server.Dispose();
        }
    }
}
