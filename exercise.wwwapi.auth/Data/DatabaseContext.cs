using exercise.wwwapi.auth.Enums;
using exercise.wwwapi.auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.auth.Data
{
    public class DatabaseContext : IdentityUserContext<ApplicationUser>
    {
        private readonly string _connectionstring;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionstring = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            ApplicationUser admin = new ApplicationUser
            {
                UserMadeUserName = "admin",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                Role = UserRole.Admin
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "admin123");
            modelBuilder.Entity<ApplicationUser>().HasData(admin);
            /*
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
                   Role = UserRole.Moderator
               },
               new ApplicationUser
               {
                   UserMadeUserName = "Flugan",
                   Email = "alice.johnson@example.com",
                   Role = UserRole.User
               }
           );*/
        }




        public DbSet<Blog> Blogs { get; set; }


    }
}