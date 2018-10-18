using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MyVendor.MyApp.Contacts
{
    public class StartupFacts : StartupFactsBase
    {
        [Fact]
        public void CanResolveContactService()
        {
            Provider.GetRequiredService<IContactService>();
        }
    }
}
