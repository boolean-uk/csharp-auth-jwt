using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        //In case of docker:
        //public static bool _migrations;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            /*if (!_migrations) { 
                this.Database.Migrate();
                _migrations = true;
            }*/
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(message => Debug.WriteLine(message)); //see the sql EF using in the console
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dinner>().HasData(
                new Dinner() {Id = 1, Name = "Lasagne", Cost = 100 },
                new Dinner() { Id = 2, Name = "Pizza", Cost = 1000 },
                new Dinner() { Id = 3, Name = "Pasta", Cost = 20 }           
                );

            base.OnModelCreating(modelBuilder);       // WHAT IS THIS:
        }

        public DbSet<Dinner> Dinners { get; set; }
    }
}
