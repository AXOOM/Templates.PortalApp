using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MyVendor.MyApp
{
    public class DatabaseFactsBase<TSubject> : AutoMockingFactsBase<TSubject>
        where TSubject : class
    {
        protected readonly DbContext Context;

        protected DatabaseFactsBase()
        {
            Context = new DbContext(
                new DbContextOptionsBuilder()
                   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use GUID so every test has its own DB
                   .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                   .EnableSensitiveDataLogging()
                   .Options);

            Use(Context);
        }

        public override void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();

            base.Dispose();
        }
    }
}
