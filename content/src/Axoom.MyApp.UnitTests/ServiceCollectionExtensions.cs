using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Axoom.MyApp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMock<T>(this IServiceCollection services, Mock<T> mock) where T : class
        {
            services.AddSingleton(mock.Object);
            return services;
        }
    }
}
