using exercise.wwwapi.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogPost>().HasKey(p => p.Id);

            //Seed Data
            modelBuilder.Entity<BlogPost>().HasData(
               new BlogPost
               {
                   Id = 1,
                   Title = "The Science of Sleep",
                   Text = "Exploring the mysteries of sleep and its impact on health and cognition.",
                   Author = "SleepEnthusiast23",
                   createdAt = DateTime.UtcNow
               },
               new BlogPost
               {
                   Id = 2,
                   Title = "The Art of Cooking",
                   Text = "Delving into the techniques and creativity behind culinary masterpieces.",
                   Author = "ChefExtraordinaire",
                   createdAt = DateTime.UtcNow
                },
               new BlogPost
               {
                   Id = 3,
                   Title = "Unraveling the Secrets of the Universe",
                   Text = "Contemplating the fundamental questions of existence and the cosmos.",
                   Author = "CosmicExplorer",
                   createdAt = DateTime.UtcNow
                });
                }

        public DbSet<BlogPost> posts { get; set; }
    }
}
