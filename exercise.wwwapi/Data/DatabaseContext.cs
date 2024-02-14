using exercise.wwwapi.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : IdentityUserContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DbData>().HasData(new DbData() { Id = 1, Description = "This is an item stored in the database, and protected through AspNetCore.Identity" });
            builder.Entity<DbData>().HasData(new DbData() { Id = 2, Description = "This is another item stored in the database, and this is also protected through AspNetCore.Identity" });
            base.OnModelCreating(builder);
        }

        public DbSet<DbData> DbData { get; set; }
    }
}
