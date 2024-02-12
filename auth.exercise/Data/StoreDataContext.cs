using Microsoft.EntityFrameworkCore;
using auth.exercise.Model;

namespace auth.exercise.Data
{
    public class StoreDataContext : DbContext
    {
        private string _connectionString;
        public StoreDataContext(DbContextOptions<StoreDataContext> options) : base(options)
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
            modelBuilder.Entity<Product>().HasKey(p => p.Id);    


            //Seed Data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 100, Quantity = 10 },
                new Product { Id = 2, Name = "Phone", Price = 200, Quantity = 20 },
                new Product { Id = 3, Name = "Keyboard", Price = 300, Quantity = 30 }
            );
        }

        public DbSet<Product> Products { get; set; }
    }
}