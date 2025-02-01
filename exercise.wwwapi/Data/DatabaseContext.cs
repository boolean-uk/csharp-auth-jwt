using System.Xml.Linq;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private IConfigurationSettings _conf;
        public DatabaseContext(DbContextOptions options, IConfigurationSettings conf) : base(options)
        {
            _conf = conf;
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFollows>()
                .HasKey(p => new { p.FollowingsId, p.FollowerId });
                


            modelBuilder.Entity<BlogPost>()
                .HasOne(p => p.User)
                .WithMany(p => p.BlogPosts)
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.BlogPost)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.BlogPostId);

            modelBuilder.Entity<UserFollows>()
                .HasOne(p => p.Follower)
                .WithMany(p => p.Followers)
                .HasForeignKey(x => x.FollowerId);

            modelBuilder.Entity<UserFollows>()
                .HasOne(p => p.Following)
                .WithMany(p => p.Followings)
                .HasForeignKey(p => p.FollowingsId);

            // Seed data
            Seeder seeder = new Seeder();
            modelBuilder.Entity<User>().
                HasData(seeder.Users);
            modelBuilder.Entity<BlogPost>().
                HasData(seeder.BlogPosts);
            modelBuilder.Entity<Comment>().
                HasData(seeder.Comments);

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_conf.GetValue<string>("ConnectionStrings:DefaultConnectionString")!);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<UserFollows> UserFollows { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
