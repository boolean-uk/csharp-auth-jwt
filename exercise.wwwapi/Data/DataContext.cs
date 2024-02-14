using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext() 
        {
            
        }

        public DataContext(DbContextOptions options ) : base( options ) 
        {
            
        }
        
        public DbSet<BlogPost> BlogPosts { get; set; }
        
    }
}
