using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Make = "Porsche", Model = "911 GT3" },
                new Car { Id = 2, Make = "Ferrari", Model = "488 Pista" },
                new Car { Id = 3, Make = "Lamborghini", Model = "Aventador SVJ" },
                new Car { Id = 4, Make = "McLaren", Model = "720S" },
                new Car { Id = 5, Make = "Bugatti", Model = "Chiron" },
                new Car { Id = 6, Make = "Aston Martin", Model = "Valkyrie" },
                new Car { Id = 7, Make = "Koenigsegg", Model = "Regera" },
                new Car { Id = 8, Make = "Pagani", Model = "Huayra BC" },
                new Car { Id = 9, Make = "Ferrari", Model = "LaFerrari" },
                new Car { Id = 10, Make = "Lamborghini", Model = "Huracan Performante" }
            );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Car> Cars { get; set; }
    }
}
