using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>()
                .HasOne(x => x.User)
                .WithMany(x => x.BlogPosts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRelation>()
                .HasKey(x => new { x.FromFollowId, x.ToFollowId });
            modelBuilder.Entity<UserRelation>()
                .HasOne(x => x.FromFollow)
                .WithMany(x => x.Following)
                .HasForeignKey(x => x.FromFollowId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRelation>()
                .HasOne(x => x.ToFollow)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.ToFollowId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<UserRelation> UserRelations { get; set; }
    }
}
