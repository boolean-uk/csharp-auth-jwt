using csharp_auth_jwt.Model;
using Microsoft.EntityFrameworkCore;

namespace csharp_auth_jwt.Data
{
    public class BlogPostContext : DbContext
    {
        public BlogPostContext(DbContextOptions<BlogPostContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost { Id = 1 , Text = "First blog post" , AuthorId = "author1" } ,
                new BlogPost { Id = 2 , Text = "Second blog post" , AuthorId = "author2" } ,
                new BlogPost { Id = 3 , Text = "Third blog post" , AuthorId = "author3" }
            );
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogUser> BlogUsers { get; set; }
    }
}
