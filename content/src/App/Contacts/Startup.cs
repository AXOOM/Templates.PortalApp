using Microsoft.Extensions.DependencyInjection;

namespace MyVendor.MyApp.Contacts
{
    public static class Startup
    {
        public static IServiceCollection AddContacts(this IServiceCollection services)
            => services.AddScoped<IContactService, ContactService>()
                       .AddSingleton<IContactMetrics, ContactMetrics>();
    }
}
