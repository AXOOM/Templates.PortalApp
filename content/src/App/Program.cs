using Microsoft.AspNetCore.Hosting;

namespace MyVendor.MyApp
{
    public static class Program
    {
        public static void Main()
            => new WebHostBuilder()
              .UseKestrel()
              .UseStartup<Startup>()
              .Build()
              .Run();
    }
}
