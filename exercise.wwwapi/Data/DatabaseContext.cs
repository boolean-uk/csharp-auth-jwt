using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;
        public DatabaseContext()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Key
            modelBuilder.Entity<BlogPost>().HasKey(b => new { b.Id });
            modelBuilder.Entity<User>().HasKey(u => new { u.Id });
            modelBuilder.Entity<Comment>().HasKey(c => new { c.Id });

            modelBuilder.Entity<BlogPost>().HasOne(b => b.User).WithMany().HasForeignKey(b => b.UserId);
            modelBuilder.Entity<BlogPost>().Navigation(b => b.User).AutoInclude();

            modelBuilder.Entity<BlogPost>().HasMany(b => b.Comments).WithOne(c => c.BlogPost).HasForeignKey(c => c.BlogPostId);
            modelBuilder.Entity<BlogPost>().Navigation(b => b.Comments).AutoInclude();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> Blogs { get; set; }
    }
}
