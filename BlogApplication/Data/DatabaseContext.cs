using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BlogApplication.Models;
using BlogApplication.Enums;

namespace BlogApplication.Data
{
    public class DatabaseContext : IdentityUserContext<ApplicationUser>
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // string g1 = Guid.NewGuid().ToString();

            //modelBuilder.Entity<ApplicationUser>().HasMany(e => e.BlogPosts).WithOne(e => e.ApplicationUser).HasForeignKey(e => e.ApplicationUserId);

            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost { Id = Guid.NewGuid().ToString(), Text = "Post 1", UserId = "teststring" }
            );
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //     base.OnConfiguring(optionsBuilder);
        //     optionsBuilder.UseInMemoryDatabase("Library");
        //}

    }
}