using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PendeloApp.Models;

namespace PendeloApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PendeloApp.Models.KmPerDay> KmPerDay { get; set; } = default!;
        public DbSet<PendeloApp.Models.Bike> Bike { get; set; } = default!;
        public DbSet<PendeloApp.Models.WorkshipSupplier> WorkshipSupplier { get; set; } = default!;
    }
}
