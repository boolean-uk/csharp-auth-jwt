using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using workshop.webapi.DataModels;

namespace workshop.webapi.Data
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
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 1, Text = "Ole Gunnar Solskjær wins it for Manchester United", AuthorId = "user1" });
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 2, Text = "Another Cristiano Hattrick", AuthorId = "henrikrosenkilde" });
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 3, Text = "January market signings, who should United target?", AuthorId = "user3" });
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 4, Text = "Jaden Sancho's time at Manchester United", AuthorId = "henrikrosenkilde" });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
