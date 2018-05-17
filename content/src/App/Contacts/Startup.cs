using Microsoft.Extensions.DependencyInjection;

namespace MyVendorName.MyAppName.Contacts
{
    public static class Startup
    {
        public static IServiceCollection AddContacts(this IServiceCollection services)
            => services.AddScoped<IContactService, ContactService>()
                       .AddSingleton<IContactMetrics, ContactMetrics>();
    }
}
