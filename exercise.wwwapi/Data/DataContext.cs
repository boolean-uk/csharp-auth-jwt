using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) 
            : base(options) 
        {
        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Seeder seeder = new Seeder();
            builder.Entity<Blogpost>().HasData(seeder.Blogposts);
            base.OnModelCreating(builder);
        }

        public DbSet<Blogpost> Blogposts { get; set; }


    }


}
