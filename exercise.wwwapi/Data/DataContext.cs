using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using exercise.wwwapi.DataModels;

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
            //further information
            //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0

            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 1, Text = "loreum ipsum", AuthorId = "1" });
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 2, Text = "blog posting on a blog post", AuthorId = "1" });
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 3, Text = "posting a post on a blog post", AuthorId = "2" });
            base.OnModelCreating(modelBuilder);
        }

        DbSet<BlogPost> BlogPosts { get; set; }
       
    }
}
