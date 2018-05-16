using VendorName.AppName.Contacts;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace VendorName.AppName
{
    public partial class MyAppDbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<PokeEntity> Pokes { get; set; }
    }
}
