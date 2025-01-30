using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Sockets;
using System.Numerics;

namespace exercise.wwwapi.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnectionString"));
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seeder seed = new Seeder();

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Author>().HasKey(a => a.Id);
            modelBuilder.Entity<BlogPost>().HasKey(b => b.Id);
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Author)
                .WithOne(a => a.User)
                .HasForeignKey<Author>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Author>()
                .HasMany(a => a.BlogPosts)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.BlogPost)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFollower>()
              .HasOne(c => c.Follower)
              .WithMany(b => b.Followers)
              .HasForeignKey(c => c.FollowerId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFollower>()
              .HasOne(c => c.Followee)
              .WithMany(b => b.Followees)
              .HasForeignKey(c => c.FolloweeId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasData(seed.Users);
            modelBuilder.Entity<Author>().HasData(seed.Authors);
            modelBuilder.Entity<BlogPost>().HasData(seed.BlogPosts);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollower> UserFollowers { get; set; }

    }
}
