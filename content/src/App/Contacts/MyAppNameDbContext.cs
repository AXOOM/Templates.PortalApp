using MyVendorName.MyAppName.Contacts;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace MyVendorName.MyAppName
{
    public partial class MyAppDbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<PokeEntity> Pokes { get; set; }
    }
}
