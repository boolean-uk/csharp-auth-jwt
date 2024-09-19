using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            //SmodelBuilder.Entity<User>().Navigation(x => x.Followers).AutoInclude();
            modelBuilder.Entity<User>().Navigation(x => x.Posts).AutoInclude();
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<Follow> Followers { get; set; }
    }
}
