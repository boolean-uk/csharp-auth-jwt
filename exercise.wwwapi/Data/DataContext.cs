using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity;

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

            modelBuilder.Entity<IdentityUserLogin<string>>()
                        .HasKey(ul => new { ul.UserId, ul.LoginProvider, ul.ProviderKey });

            modelBuilder.Entity<IdentityUserToken<string>>()
                        .HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

            modelBuilder.Entity<BlogPost>().HasData(
               new BlogPost
               {
                   Id = 1,
                   Text = "First blog post",
                   AuthorId = "author123",
                   CreatedAt = DateTimeOffset.UtcNow,
                   UpdatedAt = null
               },
               new BlogPost
               {
                   Id = 2,
                   Text = "Second blog post",
                   AuthorId = "author456",
                   CreatedAt = DateTimeOffset.UtcNow,
                   UpdatedAt = null
               }
           // Add more BlogPost data as needed
           );

        }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
