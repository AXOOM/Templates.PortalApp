using Microsoft.EntityFrameworkCore;

namespace MyVendor.MyApp
{
    /// <summary>
    /// Describes the service's database model.
    /// Used as a combination of the Unit Of Work and Repository patterns.
    /// </summary>
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        // NOTE: Other parts of this class are in separate slice-specific files

        public DbContext(DbContextOptions options)
            : base(options)
        {}
    }
}
