using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<TestHeader> TestHeaders { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<CompletedTestDto> CompletedTestsDto { get; set; }

        public DbSet<SubjectDto> subjects { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.Migrate();
            Database.EnsureCreated();
        }
    }
}
