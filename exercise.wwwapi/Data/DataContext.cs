using System;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;


namespace exercise.wwwapi.Data;

public class DataContext : DbContext
{
    private string _connectionstring;
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        _connectionstring = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;

        this.Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionstring);
        optionsBuilder.UseLazyLoadingProxies();
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        modelBuilder.Entity<Blog>()
                .HasKey(b => b.Id);

        modelBuilder.Entity<User>()
                .HasMany(u => u.Blogs)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Blog>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId);


     


    }


    
}
