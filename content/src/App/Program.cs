using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyVendor.MyApp
{
    public static class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args)
            => new WebHostBuilder().UseKestrel(x => x.Listen(IPAddress.Any, x.ApplicationServices.GetRequiredService<IConfiguration>().GetValue<int>("Port")))
                                   .UseContentRoot(Directory.GetCurrentDirectory())
                                   .UseStartup<Startup>()
                                   .Build();
    }
}