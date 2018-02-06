using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Axoom.MyApp.Controllers
{
    public abstract class ControllerTestBase : IDisposable
    {
        private readonly TestServer _server;

        protected readonly HttpClient Client;

        protected ControllerTestBase()
        {
            _server = new TestServer(new WebHostBuilder()
                .ConfigureServices(x => x.AddWeb(new ConfigurationBuilder().Build()))
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