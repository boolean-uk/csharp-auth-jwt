using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;
namespace exercise.wwwapi.Data
{
    // IMPORTANT - inherit from IdentityUserContext
    // not from DbContext
    //since identityusercontext already inherits from dbcontext we can update code with this.
    public class DatabaseContext : IdentityUserContext<Users>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder
        modelBuilder)
        {
            // IMPORTANT!: ensure we call the base class OnModelCreating
            base.OnModelCreating(modelBuilder);

            // seeders and other model definitions
            modelBuilder.Entity<Users>()
                  .HasMany(u => u.Posts)
                  .WithOne(p => p.User)
                  .HasForeignKey(p => p.UserId);

            // Seed admin user
            SeedAdminUser(modelBuilder);
        }

        public DbSet<Posts> Posts { get; set; }
        public DbSet<Users> Users { get; set; }

        private void SeedAdminUser(ModelBuilder modelBuilder)
        {
            var adminUser = new Users
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                Role = UserRole.Admin // Assuming UserRole enum has Admin as 0
            };

            PasswordHasher<Users> passwordHasher = new PasswordHasher<Users>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "admin123");

            modelBuilder.Entity<Users>().HasData(adminUser);
        }

    }
}

