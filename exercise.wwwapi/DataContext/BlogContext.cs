


using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.DataContext
{
    public class BlogContext : DbContext
    {
        private string _connectionString;

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) 
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>()
                .HasOne(a => a.User)
                .WithMany(a => a.Posts)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<RelationStatus>()
                .HasOne(u => u.Follower)
                .WithMany(u => u.AllFollowers)
                .HasForeignKey(u => u.FollowerId);

            modelBuilder.Entity<RelationStatus>()
                .HasOne(u => u.Followed)
                .WithMany()
                .HasForeignKey(u => u.FollowedId);

            modelBuilder.Entity<BlogComment>()
                .HasOne(bc => bc.BlogPost)
                .WithMany(bc => bc.BlogComments)
                .HasForeignKey(bc => bc.BlogPostId);

            modelBuilder.Entity<BlogComment>()
                .HasOne(bc => bc.User)
                .WithMany(bc => bc.Comments)
                .HasForeignKey(bc => bc.UserId);
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> Blogs { get; set; }

        public DbSet<RelationStatus> RelationStatuses { get; set; }
    }
}
