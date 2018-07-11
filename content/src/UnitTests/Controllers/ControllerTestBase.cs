using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyVendor.MyApp.Infrastructure;
using Xunit.Abstractions;

namespace MyVendor.MyApp.Controllers
{
    public abstract class ControllerTestBase : IDisposable
    {
        private readonly TestServer _server;

        protected readonly HttpClient Client;

        protected ControllerTestBase(ITestOutputHelper output)
        {
            _server = new TestServer(new WebHostBuilder()
                                    .ConfigureServices(x => x.AddLogging(builder => builder.AddXunit(output))
                                                             .AddWeb(new ConfigurationBuilder().Build()))
                                    .ConfigureServices(ConfigureService)
                                    .Configure(x => x.UseWeb()));
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
