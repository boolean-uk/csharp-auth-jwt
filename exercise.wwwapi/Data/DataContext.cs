using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlogPost>().HasData(new BlogPost { Id = 1, Text = "First blogpost", AuthorId = "Some author" });
            base.OnModelCreating(builder);
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
