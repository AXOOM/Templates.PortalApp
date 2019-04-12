using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MyVendor.MyApp
{
    /// <summary>
    /// Instantiates a test <typeparamref name="TSubject"/>, injecting an in-memory database and mocks for its other dependencies.
    /// </summary>
    public class DatabaseFactsBase<TSubject> : AutoMockingFactsBase<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// An in-memory database that is reset after every test.
        /// </summary>
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

        /// <summary>
        /// Adds one or more entity to the database and then detaches them from the <see cref="Context"/>.
        /// This is useful to prefill the database with seed data while keeping the <see cref="Context"/> in a pristine state for the actual test.
        /// </summary>
        protected void AddDetached<T>(params T[] entities)
        {
            var entries = entities.Select(entity => Context.Add(entity)).ToList();
            Context.SaveChanges();
            foreach (var entry in entries)
                entry.State = EntityState.Detached;
        }
    }
}
