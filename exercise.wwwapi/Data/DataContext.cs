using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

            //keys

            //Seeding
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost { Id = 1, Title = "How to NOT become a pro", Text = "This is not worth reading" });
            base.OnModelCreating(modelBuilder);
        }
        DbSet<BlogPost> BlogPosts { get; set; }
    }
}
