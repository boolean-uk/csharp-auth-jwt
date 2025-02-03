using System.Numerics;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data;

public class DataContext : DbContext
{
    private string connectionString;

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        connectionString = configuration.GetValue<string>(
            "ConnectionStrings:DefaultConnectionString"
        )!;
        this.Database.EnsureCreated();
        this.Database.SetConnectionString(connectionString);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies().UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<User>().HasKey(c => c.Id);
        m.Entity<User>().HasMany(c => c.Posts).WithOne().HasForeignKey(t => t.AuthorId);
        m.Entity<User>()
            .HasMany(u => u.Following)
            .WithOne(f => f.Follower)
            .HasForeignKey(f => f.FollowerId);

        m.Entity<User>()
            .HasMany(u => u.FollowedBy)
            .WithOne(f => f.Followee)
            .HasForeignKey(f => f.FolloweeId);

        m.Entity<BlogPost>().HasKey(c => c.Id);
    }

    public DbSet<BlogPost> Blogs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }
}
