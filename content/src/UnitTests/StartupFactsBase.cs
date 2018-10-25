using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyVendor.MyApp
{
    public abstract class StartupFactsBase
    {
        protected readonly IServiceProvider Provider;

        protected StartupFactsBase(IDictionary<string, string> configuration = null)
        {
            var configBuilder = new ConfigurationBuilder().AddInMemoryCollection(configuration);
            if (configuration != null)
                configBuilder.AddInMemoryCollection(configuration);

            var services = new ServiceCollection();
            new Startup(configBuilder.Build()).ConfigureServices(services);
            Provider = services.BuildServiceProvider();
        }
    }
}
