using Microsoft.EntityFrameworkCore;

namespace Axoom.MyApp
{
    /// <summary>
    /// Describes the service's database model.
    /// Used as a combination of the Unit Of Work and Repository patterns.
    /// </summary>
    public partial class MyAppDbContext : DbContext
    {
        // NOTE: Other parts of this class are in separate slice-specific files

        public MyAppDbContext(DbContextOptions options)
            : base(options)
        {}
    }
}
