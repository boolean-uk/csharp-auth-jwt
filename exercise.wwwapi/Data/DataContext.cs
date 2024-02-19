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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlogPost>().HasData(new BlogPost { Id = 1, Text = "Heisan bloggen! Idag skal vi prøve ut den nye buljongen fra Coop Minus!", AuthorId = 1 });
            builder.Entity<BlogPost>().HasData(new BlogPost { Id = 2, Text = "Heisan bloggen! HAn BrukTe TanNBørStin MiN", AuthorId = 1 });
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        
    }
}
