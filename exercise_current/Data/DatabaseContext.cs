using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Numerics;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : IdentityUserContext<ApplicationUser>
    {
        private string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Posts>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<Posts>().HasData(
                new Posts { Id = 1, AuthorId = 1, Text = "This is a post"}
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Victor" }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message)); //see the sql EF using in the console
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Posts> Posts { get; set; }
    }
}
