using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Numerics;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using WebApplication1.Models;
using System.Net.Sockets;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using WebApplication1.Data;

namespace WebApplication1.Data
{
    public class BlogContext : IdentityUserContext<AuthUser>
    {
        private string _connectionString;
        public DbSet<BlogPost> BlogPosts { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            _connectionString = configuration
                .GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message)); //see the sql EF using in the console
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BlogPost>().HasData
                (
                new BlogPost
                {
                    Id = 1,
                    Title = "Why are elephants so big?",
                    Text = "Probably because they eat so much",
                    Author = "Crazyflows88",
                    CreatedAt = DateTime.UtcNow
                },

                new BlogPost
                {
                    Id = 2,
                    Title = "The Art of Minimalism",
                    Text = "Simplify your life, declutter your mind.",
                    Author = "SimplicityFanatic",
                    CreatedAt = DateTime.UtcNow
                },

                new BlogPost
                {
                    Id = 3,
                    Title = "Exploring the Universe",
                    Text = "Discovering the mysteries beyond our planet.",
                    Author = "Stargazer123",
                    CreatedAt = DateTime.UtcNow
                },

                new BlogPost
                {
                    Id = 4,
                    Title = "The Power of Meditation",
                    Text = "Finding inner peace through mindfulness.",
                    Author = "ZenMaster",
                    CreatedAt = DateTime.UtcNow
                },

                new BlogPost
                {
                    Id = 5,
                    Title = "Cooking with Passion",
                    Text = "Bringing love to the kitchen, one dish at a time.",
                    Author = "ChefExtraordinaire",
                    CreatedAt = DateTime.UtcNow
                },

                new BlogPost
                {
                    Id = 6,
                    Title = "Traveling on a Budget",
                    Text = "Exploring the world without breaking the bank.",
                    Author = "BudgetTraveler",
                    CreatedAt = DateTime.UtcNow
                },

                new BlogPost
                {
                    Id = 7,
                    Title = "The Joy of Reading",
                    Text = "Unlocking new worlds with every page turned.",
                    Author = "Bookworm",
                    CreatedAt = DateTime.UtcNow
                },

                new BlogPost
                {
                    Id = 8,
                    Title = "Embracing Change",
                    Text = "Embracing change is the key to personal growth.",
                    Author = "ChangeAgent",
                    CreatedAt = DateTime.UtcNow
                }
                );
        }
    }
}
