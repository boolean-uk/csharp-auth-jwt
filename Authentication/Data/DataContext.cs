using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Authentication.Model;

namespace Authentication.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DbSet<BlogPost> Posts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost { Id = 1, Title = "News", Description = "The current news", Author = "Jane Pettersson" },
                new BlogPost { Id = 2, Title = "Cleaner supplies", Description = "Supplies to clean with", Author = "Petter Johnsson" },
                new BlogPost { Id = 3, Title = "Toys", Description = "Things to play with", Author = "Claire Sarah" }
            );
        }

    }
}