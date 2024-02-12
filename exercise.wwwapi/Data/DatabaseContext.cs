using System.Diagnostics;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : IdentityUserContext<ApplicationUser>
    {
        private string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext>options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            // seeders and other model definitions
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Blogpost> Blogposts { get; set; }
    }

}
