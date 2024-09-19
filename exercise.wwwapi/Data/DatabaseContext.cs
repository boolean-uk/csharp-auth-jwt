using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>().Navigation(x => x.Author).AutoInclude();

            //modelBuilder.Entity<BlogPost>().HasData(new List<BlogPost>
            //{
            //    new BlogPost() { Id = 1, Title = "Introduction to C#", Content = "Learn the basics of C# programming language and its features.", AuthorId = 1 },
            //    new BlogPost() { Id = 2, Title = "Understanding OOP Concepts", Content = "Explore the core concepts of Object-Oriented Programming in C#.", AuthorId = 2 },
            //    new BlogPost() { Id = 3, Title = "Working with LINQ", Content = "Discover how to use LINQ for data manipulation in C#.", AuthorId = 2 },
            //    new BlogPost() { Id = 4, Title = "Asynchronous Programming in C#", Content = "An overview of async and await keywords for asynchronous programming.", AuthorId = 2 },
            //    new BlogPost() { Id = 5, Title = "Building Web APIs with ASP.NET Core", Content = "Step-by-step guide to creating RESTful APIs using ASP.NET Core.", AuthorId = 3 },
            //    new BlogPost() { Id = 6, Title = "Entity Framework Core Basics", Content = "Introduction to Entity Framework Core for database operations in C#.", AuthorId = 1 },
            //    new BlogPost() { Id = 7, Title = "Unit Testing in C#", Content = "Learn how to write and run unit tests in C# using popular frameworks.", AuthorId = 4 },
            //    new BlogPost() { Id = 8, Title = "Dependency Injection in .NET", Content = "Understand the principles and implementation of Dependency Injection in .NET applications.", AuthorId = 3 },
            //    new BlogPost() { Id = 9, Title = "Advanced C# Features", Content = "Dive into advanced features of C# like delegates, events, and lambda expressions.", AuthorId = 1 },
            //    new BlogPost() { Id = 10, Title = "Deploying .NET Applications", Content = "Best practices for deploying .NET applications to various environments.", AuthorId = 4 }
            //});
        }

        public DbSet<BlogPost> blogPosts { get; set; }
        public DbSet<User> users { get; set; }
    }
}
