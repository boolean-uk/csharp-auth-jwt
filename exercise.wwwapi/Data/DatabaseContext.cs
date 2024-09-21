using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;
        public DatabaseContext()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection")!;


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorFollower>()
            .HasKey(af => new { af.FollowerId, af.FollowedId });

            modelBuilder.Entity<AuthorFollower>()
                .HasOne(af => af.Follower)
                .WithMany(a => a.Following)
                .HasForeignKey(af => af.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuthorFollower>()
                .HasOne(af => af.Followed)
                .WithMany(a => a.Followers)
                .HasForeignKey(af => af.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne<Author>()
                .WithMany(a => a.Comments)
                .HasForeignKey(a => a.authorId);

            modelBuilder.Entity<Comment>()
                .HasOne<BlogPost>()
                .WithMany(a => a.comments)
                .HasForeignKey(b => b.BlogPostId);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BlogPost> Posts { get; set; }

        public DbSet<AuthorFollower> Followers { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
