using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace exercise.wwwapi.Data
{
    public class DataContext : DbContext
    {
        private string connectionString;
        public DataContext(DbContextOptions<DataContext> options) : base(options) { 
        
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString");

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
   
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; } 
    }
}
