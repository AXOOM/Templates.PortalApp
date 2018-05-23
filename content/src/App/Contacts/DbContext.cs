using Microsoft.EntityFrameworkCore;
using MyVendor.MyApp.Contacts;

// ReSharper disable once CheckNamespace
namespace MyVendor.MyApp
{
    public partial class DbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<PokeEntity> Pokes { get; set; }
    }
}
