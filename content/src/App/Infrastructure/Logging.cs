using System;
using Axoom.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyVendorName.MyAppName.Infrastructure
{
    public static class Logging
    {
        public static IServiceCollection AddAxoomLogging(this IServiceCollection services, IConfiguration configration)
            => services.AddLogging(builder => builder.AddConfiguration(configration.GetSection("Logging")));

        public static void UseAxoomLogging(this IServiceProvider provider)
            => provider.GetRequiredService<ILoggerFactory>()
                       .AddAxoomConsole(provider.GetRequiredService<IConfiguration>().GetSection("Logging"))
                       .CreateLogger("Startup")
                       .LogInformation("Starting My App");
    }
}
