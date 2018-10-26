using System.IO;
using Axoom.Extensions.Logging.Console;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MyVendor.MyApp
{
    /// <summary>
    /// Manages process lifetime, configuration and logging.
    /// </summary>
    public static class Program
    {
        public static void Main()
            => new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .ConfigureAppConfiguration((context, builder) =>
               {
                   var env = context.HostingEnvironment;
                   builder.SetBasePath(env.ContentRootPath)
                          .AddYamlFile("appsettings.yml", optional: false, reloadOnChange: true)
                          .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true, reloadOnChange: true)
                          .AddEnvironmentVariables();
               })
              .ConfigureLogging((context, builder) =>
               {
                   var config = context.Configuration.GetSection("Logging");
                   builder.AddConfiguration(config)
                          .AddAxoomConsole(config)
                          .AddExceptionDemystifyer();
               })
              .UseStartup<Startup>()
              .Build()
              .Run();
    }
}
