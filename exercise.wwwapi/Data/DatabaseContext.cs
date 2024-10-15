using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Emit;
using exercise.wwwapi.Models;

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
            modelBuilder.Entity<BlogPost>()
               .HasKey(b => b.Id); 

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id); 

            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.Author)
                .WithMany() 
                .HasForeignKey(b => b.AuthorId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<User> Users { get; set; }

    }
}