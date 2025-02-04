using System.Net.Sockets;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data

{
    public class DataContext : DbContext
    {
        private string _connectionString;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
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

            // Relationships
            modelBuilder.Entity<BlogPost>()
                .HasOne<User>(bp => bp.Author)
                .WithMany(u => u.Blogs)
                .HasForeignKey(bp => bp.authorId);
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<User> Users { get; set; }

    }

}