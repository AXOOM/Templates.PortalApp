using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MyVendor.MyApp.Contacts
{
    public class StartupFacts : StartupFactsBase
    {
        public StartupFacts() : base(new Dictionary<string, string>
        {
            ["Database:ConnectionString"] = ":memory:"
        })
        {}

        [Fact]
        public void CanResolveContactService()
        {
            Provider.GetRequiredService<IContactService>();
        }
    }
}
