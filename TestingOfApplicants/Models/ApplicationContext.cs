using Microsoft.EntityFrameworkCore;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TestHeader> TestHeaders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.Migrate();
            Database.EnsureCreated();
        }
    }
}
