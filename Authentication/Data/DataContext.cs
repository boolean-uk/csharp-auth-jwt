using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Authentication.Model;
using Authentication.Enums;
using System;

namespace Authentication.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DbSet<BlogPost> Posts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var hasher = new PasswordHasher<ApplicationUser>();

            ApplicationUser user = new ApplicationUser { UserName = "klara", NormalizedUserName = "KLARA", Email = "klara@gmail.com", NormalizedEmail = "KLARA@GMAIL.COM", Role = UserRole.Administrator };
            user.PasswordHash = hasher.HashPassword(user, "test_pwd");
            modelBuilder.Entity<ApplicationUser>().HasData(user);
        }
    }
}