using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
       : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<blogPost>().HasData(new blogPost { Id = 1, Text = "BlogBlogBlog", AuthorId = "Jensemann" });
            modelBuilder.Entity<blogPost>().HasData(new blogPost { Id = 2, Text = "AnotherBlog", AuthorId = "Henrik" });
            modelBuilder.Entity<blogPost>().HasData(new blogPost { Id = 3, Text = "AnotherAnotherBlog", AuthorId = "Kristian" });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<blogPost> BlogPosts { get; set; }
    }
}
