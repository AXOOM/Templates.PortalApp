using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Axoom.MyApp
{
    public static class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args)
            => new WebHostBuilder().UseKestrel()
                                   .UseContentRoot(Directory.GetCurrentDirectory())
                                   .UseStartup<Startup>()
                                   .Build();
    }
}
