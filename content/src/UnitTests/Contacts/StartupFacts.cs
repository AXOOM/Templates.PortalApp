using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace MyVendor.MyApp.Contacts
{
    public class StartupFacts : StartupFactsBase
    {
        public StartupFacts(ITestOutputHelper output)
            : base(output, new Dictionary<string, string>
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
