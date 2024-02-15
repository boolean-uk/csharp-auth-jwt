using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        private string _connectionString;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 1, AuthorId = "1", Text = "abc"});
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = "1", Email = "mail.com", Role = Role.User});
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
