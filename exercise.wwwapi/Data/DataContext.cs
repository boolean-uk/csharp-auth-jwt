using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);  
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
