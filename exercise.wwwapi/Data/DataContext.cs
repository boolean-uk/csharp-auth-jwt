using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace workshop.webapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
       : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //further information
            //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0


            modelBuilder.Entity<Car>().HasData(new Car { Id = 1, Make = "Mini", Model = "Clubman" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 2, Make = "VW", Model = "T5 California" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 3, Make = "VW", Model = "Up" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 4, Make = "VW", Model = "id5" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 5, Make = "VW", Model = "Golf" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 6, Make = "VW", Model = "Beetle" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 7, Make = "VW", Model = "Polo" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 8, Make = "Smart", Model = "ForTwo" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 9, Make = "VW", Model = "Bournemouth" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 10, Make = "VW", Model = "Down" });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Car> Cars { get; set; }
    }
}