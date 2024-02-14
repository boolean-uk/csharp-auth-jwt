using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
       : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost { Id = 1, Text = "First Blog Post", AuthorId = "Author1" },
                new BlogPost { Id = 2, Text = "Exploring AI", AuthorId = "Author2" },
                new BlogPost { Id = 3, Text = "Machine Learning 101", AuthorId = "Author3" },
                new BlogPost { Id = 4, Text = "Deep Dive into Neural Networks", AuthorId = "Author4" },
                new BlogPost { Id = 5, Text = "Understanding Natural Language Processing", AuthorId = "Author5" },
                new BlogPost { Id = 6, Text = "The Future of AI", AuthorId = "Author6" },
                new BlogPost { Id = 7, Text = "AI in Healthcare", AuthorId = "Author7" },
                new BlogPost { Id = 8, Text = "AI in Autonomous Vehicles", AuthorId = "Author8" },
                new BlogPost { Id = 9, Text = "AI in Finance", AuthorId = "Author9" },
                new BlogPost { Id = 10, Text = "Ethics in AI", AuthorId = "Author10" }
            );

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<BlogPost> Cars { get; set; }
    }
}
