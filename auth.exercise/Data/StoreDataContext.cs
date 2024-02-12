using Microsoft.EntityFrameworkCore;
using auth.exercise.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace auth.exercise.Data
{
    public class StoreDataContext : IdentityUserContext<ApplicationUser>
    {
       
        public StoreDataContext(DbContextOptions<StoreDataContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);

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