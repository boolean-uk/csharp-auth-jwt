using exercise.wwwapi.auth.Enums;
using exercise.wwwapi.auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.auth.Data
{
    public class DatabaseContext : IdentityUserContext<ApplicationUser>
    {
        private readonly string _connectionstring;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionstring = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().HasData(
               new ApplicationUser
               {
                   UserMadeUserName = "JojoDoe", 
                   Email = "john.doe@example.com",
                   Role = UserRole.Admin
               },
               new ApplicationUser
               {
                   UserMadeUserName = "SmithyJane",
                   Email = "jane.smith@example.com",
                   Role = UserRole.Modirator
               },
               new ApplicationUser
               {
                   UserMadeUserName = "Flugan",
                   Email = "alice.johnson@example.com",
                   Role = UserRole.User
               }
           );
        }


    }
}
