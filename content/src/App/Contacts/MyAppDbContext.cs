using Axoom.MyApp.Contacts;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Axoom.MyApp
{
    public partial class MyAppDbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<PokeEntity> Pokes { get; set; }
    }
}
