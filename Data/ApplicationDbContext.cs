
using CampusFix.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusFix.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MaintenanceRequest> MaintenanceRequests
            => Set<MaintenanceRequest>();

        public DbSet<Category> Categories
            => Set<Category>();
    }
}
