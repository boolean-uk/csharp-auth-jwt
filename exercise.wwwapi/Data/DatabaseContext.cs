using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private IConfigurationSettings _conf;
        public DatabaseContext(DbContextOptions options, IConfigurationSettings conf) : base(options)
        {
            _conf = conf;
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BlogPost>()
                .HasOne(p => p.User)
                .WithMany(p => p.BlogPosts)
                .HasForeignKey(p => p.AuthorId);

            // Seed data
            Seeder seeder = new Seeder();
            modelBuilder.Entity<User>().
                HasData(seeder.Users);
            modelBuilder.Entity<BlogPost>().
                HasData(seeder.BlogPosts);

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_conf.GetValue<string>("ConnectionStrings:DefaultConnectionString")!);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
