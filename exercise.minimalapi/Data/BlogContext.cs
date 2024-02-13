using exercise.minimalapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
