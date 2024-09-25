using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DataBaseContext : DbContext
    {
        private string _connectionString;
        public DataBaseContext()
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
                .HasOne(b => b.Author)
                .WithMany(b => b.BlogPosts)
                .HasForeignKey(b => b.AuthorId);
            /*
            modelBuilder.Entity<BlogPost>()
                .Navigation(x => x.Author)
                .AutoInclude();
            */

            modelBuilder.Entity<UserRelations>()
                .HasOne(u => u.Follower)
                .WithMany(u => u.UserRelations)
                .HasForeignKey(u => u.FollowerId);

            modelBuilder.Entity<UserRelations>()
                .HasOne(u => u.Followed)
                .WithMany() //Followed user dont need to see all the followers
                .HasForeignKey(u => u.FollowedId);

            modelBuilder.Entity<UserRelations>()
                .Navigation(x => x.Follower)
            .AutoInclude();

            modelBuilder.Entity<BlogComments>()
                .HasOne(bc => bc.BlogPost)
                .WithMany(bc => bc.BlogComments)
                .HasForeignKey(bc => bc.BlogPostId);

            modelBuilder.Entity<BlogComments>()
                .HasOne(bc => bc.Author)
                .WithMany(bc => bc.BlogComments)
                .HasForeignKey(bc => bc.AuthorId);

            /*
            modelBuilder.Entity<BlogComment>()
                .Navigation(x => x.BlogPost)
                .AutoInclude();

            modelBuilder.Entity<BlogComment>()
                .Navigation(x => x.Author)
                .AutoInclude();
            */
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<UserRelations> UserRelations { get; set; }
        public DbSet<BlogComments> BlogComments { get; set; }
    }

}
