using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<Author>
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
//            Seeder seeder = new Seeder();
//           modelBuilder.Entity<BlogPost>().HasData(seeder.BlogPosts);
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
