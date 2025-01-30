using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string connectionString;
        public DatabaseContext()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString") ?? "";
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "user1@example.com", HashedPassword = Util.HashPassword("password1") },
                new User { Id = 2, Email = "user2@example.com", HashedPassword = Util.HashPassword("password2") }
            );

            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost { Id = 1, Title = "First Blog", Text = "This is my first blog post!", UserId = 1 },
                new BlogPost { Id = 2, Title = "Second Blog", Text = "This is my second blog post!", UserId = 1 },
                new BlogPost { Id = 3, Title = "Third Blog", Text = "This is my third blog post!", UserId = 2 }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        
    }
}
