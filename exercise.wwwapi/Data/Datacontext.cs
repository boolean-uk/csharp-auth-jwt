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

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogpPosts { get; set; }
    }
}