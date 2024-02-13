using Microsoft.EntityFrameworkCore;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext :DbContext
    {
        private string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlogPost>().HasData(
                new BlogPost {Id = 1, Author = "Percy Jackson", Text = "Look I didn't want to be a halfblood"},
                new BlogPost { Id = 2, Author = "Annabeth Chase", Text = "You drool in your sleep" },
                new BlogPost { Id = 3, Author = "Nico di Angelo", Text = "With great power, comes a great need to take a nap. Wake me up later" }


                );
            
        }

        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
    }
}
