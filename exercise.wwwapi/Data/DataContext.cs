using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Sockets;
using System.Numerics;

namespace exercise.wwwapi.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnectionString"));
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seeder seed = new Seeder();

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Author>().HasKey(a => a.Id);
            modelBuilder.Entity<BlogPost>().HasKey(b => b.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Author) 
                .WithOne(a => a.User) 
                .HasForeignKey<Author>(a => a.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.BlogPosts) 
                .WithOne(b => b.Author) 
                .HasForeignKey(b => b.AuthorId) 
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<User>().HasData(seed.Users);
            modelBuilder.Entity<Author>().HasData(seed.Authors);
            modelBuilder.Entity<BlogPost>().HasData(seed.BlogPosts);

        }

        public DbSet<User> Users { get; set; }
    }
}
