using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Numerics;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace exercise.wwwapi.Data
{
    public class DataContext : DbContext
    {
        private string _connectionString;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.SetConnectionString(_connectionString);
            this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
           optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseLazyLoadingProxies();

            

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasKey(b => b.postId);
            modelBuilder.Entity<User>()
                .HasKey(b => b.userId);
            modelBuilder.Entity<Comment>()
                .HasKey(b => b.commentId);

            modelBuilder.Entity<Post>()
                 .HasOne(b => b.user)
                 .WithMany(b => b.Posts)
                 .HasForeignKey(b => b.userId);

            modelBuilder.Entity<Post>()
                 .HasMany(b => b.comments)
                 .WithOne(b => b.post);
        

            modelBuilder.Entity<User>()
                .HasMany(b => b.Posts)
                .WithOne(b => b.user);

            modelBuilder.Entity<User>()
                 .HasMany(b => b.Comments)
                 .WithOne(b => b.user);


            modelBuilder.Entity<Comment>()
                .HasOne(b => b.user)
                .WithMany(c => c.Comments)
                .HasForeignKey(b => b.userId);

            modelBuilder.Entity<Comment>()
                .HasOne(b => b.post)
                .WithMany(c => c.comments)
                .HasForeignKey(b => b.postId);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}