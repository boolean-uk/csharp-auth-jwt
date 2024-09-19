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
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
            optionsBuilder.UseNpgsql(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => new { x.Id });
            
            modelBuilder.Entity<BlogPost>().HasKey(x => new { x.Id });
            modelBuilder.Entity<BlogPost>().HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId);
            modelBuilder.Entity<BlogPost>().Navigation(p => p.User).AutoInclude();


        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
