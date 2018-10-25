using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MyVendor.MyApp
{
    public static class Program
    {
        public static void Main()
            => new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseStartup<Startup>()
              .Build()
              .Run();
    }
}
