using exercise.wwwapi.Model;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.DataContext
{
    public class BlogContext : DbContext
    {
        private string _connectionString;

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.Author)
                .WithMany(b => b.BlogPosts)
                .HasForeignKey(b => b.AuthorId);
            /*
            modelBuilder.Entity<BlogPost>()
                .Navigation(x => x.Author)
                .AutoInclude();
            */

            modelBuilder.Entity<UserRelationStatus>()
                .HasOne(u => u.Follower)
                .WithMany(u => u.UserRelations)
                .HasForeignKey(u => u.FollowerId);

            modelBuilder.Entity<UserRelationStatus>()
                .HasOne(u => u.Followed)
                .WithMany() //Followed user dont need to see all the followers
                .HasForeignKey(u => u.FollowedId);

            modelBuilder.Entity<UserRelationStatus>()
                .Navigation(x => x.Follower)
                .AutoInclude();

            modelBuilder.Entity<BlogComment>()
                .HasOne(bc => bc.BlogPost)
                .WithMany(bc => bc.BlogComments)
                .HasForeignKey(bc => bc.BlogPostId);

            modelBuilder.Entity<BlogComment>()
                .HasOne(bc => bc.Author)
                .WithMany(bc => bc.BlogComments)
                .HasForeignKey(bc => bc.AuthorId);

            /*
            modelBuilder.Entity<BlogComment>()
                .Navigation(x => x.BlogPost)
                .AutoInclude();

            modelBuilder.Entity<BlogComment>()
                .Navigation(x => x.Author)
                .AutoInclude();
            */
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<UserRelationStatus> UserRelations { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
    }
}
