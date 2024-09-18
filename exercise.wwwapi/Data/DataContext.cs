using exercise.wwwapi.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        private string _connectionString;
        public DataContext(DbContextOptions<DataContext> options)
       : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            //this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //further information
            //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0


            modelBuilder.Entity<Student>().HasData(new Student { Id = 1, FirstName = "Sebastian", LastName = "Hanssen" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 2, FirstName = "Philip", LastName = "Morud" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 3, FirstName = "Aleksander", LastName = "Kolsrud" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 4, FirstName = "Nigel", LastName = "Sibbert" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 5, FirstName = "Mikolaj", LastName = "Baran" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 6, FirstName = "Santhia", LastName = "Shan" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 7, FirstName = "Kristian", LastName = "Hjelmtveit" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 8, FirstName = "Moxnes", LastName = "Greeven" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 9, FirstName = "Edvard", LastName = "Helgetun" });
            modelBuilder.Entity<Student>().HasData(new Student { Id = 10, FirstName = "Sebastian", LastName = "Ankill" });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Student> Students { get; set; }
    }
}
