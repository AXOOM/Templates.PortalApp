using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyVendor.MyApp.Infrastructure;
using Xunit.Abstractions;

namespace MyVendor.MyApp.Controllers
{
    public abstract class ControllerTestBase : IDisposable
    {
        private readonly TestServer _server;

        protected readonly HttpClient Client;

        protected ControllerTestBase(ITestOutputHelper output, IDictionary<string, string> configuration = null)
        {
            _server = new TestServer(
                new WebHostBuilder()
                   .ConfigureAppConfiguration(builder =>
                    {
                        if (configuration != null)
                            builder.AddInMemoryCollection(configuration);
                    })
                   .ConfigureLogging(builder => builder.AddXUnit(output))
                   .ConfigureServices((context, services) => services.AddWeb(context.Configuration))
                   .ConfigureServices(ConfigureService)
                   .Configure(builder => builder.UseWeb()));

            Client = _server.CreateClient();
        }

        protected abstract void ConfigureService(IServiceCollection services);

        public virtual void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
