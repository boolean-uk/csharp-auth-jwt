using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Authentication.Model;
using Authentication.Enums;

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
        }
    }
}