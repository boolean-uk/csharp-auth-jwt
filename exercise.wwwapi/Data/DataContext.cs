using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Diagnostics;


namespace exercise.wwwapi.Data
{
    // IMPORTANT - inherit from IdentityUserContext
    // not from DbContext
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public string _connectionString;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); //needed for ElephantSQL, but not for docker
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT!: ensure we call the base class 
            base.OnModelCreating(modelBuilder);


            // seeders and other model definitions
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "John Doe" }
                );
            modelBuilder.Entity<Blogpost>().HasData(
                new Blogpost { Id = 1, Title =  "first post", Text = "Test", AuthorId = 1 }
                );
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Blogpost> Posts { get; set; }
    }

}
