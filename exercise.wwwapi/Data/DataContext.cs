using exercise.wwwapi.DataModels;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Used for creating a composite key of user that follows, and user getting followed
            modelBuilder.Entity<Follow>().HasKey(k => new { k.UserId, k.OtherUserId });

            // Need to declare that we have a single instance of both a user, and another user
            modelBuilder.Entity<Follow>().HasOne(u => u.User);
            modelBuilder.Entity<Follow>().HasOne(u => u.OtherUser);

            // This is used for automatically including blogposts from a given user
            modelBuilder.Entity<User>().HasMany(u => u.BlogPosts).WithOne(b => b.Author);
            modelBuilder.Entity<User>().Navigation(x => x.BlogPosts).AutoInclude();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Follow> Follows { get; set; }
    }
}
