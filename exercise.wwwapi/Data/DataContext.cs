using System.Net.Sockets;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data

{
    public class DataContext : DbContext
    {
        private string _connectionString;
        public DataContext()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<BlogPost>().ToTable("blog_posts");
            modelBuilder.Entity<Comment>().ToTable("comments");

            // Relationships
            modelBuilder.Entity<BlogPost>()
                .HasOne<User>(bp => bp.Author)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(bp => bp.AuthorId);

            modelBuilder.Entity<Comment>()
                .HasOne<BlogPost>(c => c.BlogPost)
                .WithMany(bp => bp.Comments)
                .HasForeignKey(c => c.BlogPostId);

            modelBuilder.Entity<Comment>()
                .HasOne<User>(c => c.Commenter)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.CommenterId);

        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<User> Users { get; set; }

    }

}
