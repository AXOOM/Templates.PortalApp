using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace MyVendor.MyApp
{
    public class StartupFacts
    {
        private readonly ITestOutputHelper _output;

        private readonly IConfiguration _configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            ["Database:ConnectionString"] = ":memory:"
        }).Build();

        private readonly ServiceCollection _services = new ServiceCollection();
        private readonly IServiceProvider _provider;

        public StartupFacts(ITestOutputHelper output)
        {
            _output = output;

            AddFrameworkServices();
            _services.AddLogging(builder => builder.AddXUnit());
            new Startup(_configuration).ConfigureServices(_services);

            _provider = _services.BuildServiceProvider();
        }

        private void AddFrameworkServices()
            => _services.AddSingleton<IHostingEnvironment>(new HostingEnvironment {ContentRootPath = "dummy"})
                        .AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>()
                        .AddSingleton(new Mock<DiagnosticSource>().Object);

        [Fact]
        public void CanResolveAllRegisteredServices()
        {
            foreach (var type in _services.Select(x => x.ServiceType).Where(x => !x.IsGenericTypeDefinition))
            {
                _output.WriteLine("Resolving {0}", type);
                _provider.GetRequiredService(type);
            }
        }
    }
}
