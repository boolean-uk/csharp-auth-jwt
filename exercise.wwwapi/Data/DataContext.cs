using exercise.wwwapi.Data.Seed;
using exercise.wwwapi.Models.PureModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            Seeder seed = new Seeder();

            modelBuilder.Entity<ApplicationUser>().HasData(seed.Users);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }

    }
}
