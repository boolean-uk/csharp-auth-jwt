using exercise.minimalapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace exercise.minimalapi.Data
{
    public class BlogContext : IdentityUserContext<ApplicationUser>
    {
        
            public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder) 
            { 
                base.OnModelCreating(modelBuilder); 
            }
        
        

        public DbSet<Post> BlogPosts { get; set; }
    }
}
