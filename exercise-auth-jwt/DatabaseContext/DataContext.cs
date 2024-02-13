using exercise_auth_jwt.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Diagnostics;


namespace exercise_auth_jwt.DatabaseContext
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {

        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BlogPost>().HasKey(e => new {e.Id });

        }


        public DbSet<BlogPost> BlogPosts { get; set; }

    }
}
