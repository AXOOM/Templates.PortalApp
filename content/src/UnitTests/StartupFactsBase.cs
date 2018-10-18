using System;
using Microsoft.Extensions.DependencyInjection;

namespace MyVendor.MyApp
{
    public abstract class StartupFactsBase
    {
        protected readonly IServiceProvider Provider;

        protected StartupFactsBase()
        {
            var services = new ServiceCollection();
            new Startup().ConfigureServices(services);
            Provider = services.BuildServiceProvider();
        }
    }
}
