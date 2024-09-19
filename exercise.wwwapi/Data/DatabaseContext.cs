using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;

        public DatabaseContext()
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
            modelBuilder.Entity<Blogpost>().Navigation(x => x.Author).AutoInclude();
            modelBuilder.Entity<Blogpost>().Navigation(x => x.Comments).AutoInclude();
            modelBuilder.Entity<Comment>().Navigation(x => x.Author).AutoInclude();

            List<User> users = new List<User>()
            {
                new User() { Id = 1, Username = "espensl", PasswordHash = BCrypt.Net.BCrypt.HashPassword("passord")},
                new User() { Id = 2, Username = "eyvindmal", PasswordHash = BCrypt.Net.BCrypt.HashPassword("passord")},
                new User() { Id = 3, Username = "danielrol", PasswordHash = BCrypt.Net.BCrypt.HashPassword("passord")},
            };

            List<Blogpost> posts = new List<Blogpost>()
            {
                new Blogpost() { Id = 1, AuthorId = 1, Title = "Innlegg 1 espen", Content = "Innlegg 1 content espen" },
                new Blogpost() { Id = 2, AuthorId = 1, Title = "Innlegg 2 espen", Content = "Innlegg 2 content espen" },
                new Blogpost() { Id = 3, AuthorId = 2, Title = "Innlegg 1 eyvind", Content = "Innlegg 1 content eyvind" }
            };

            List<Comment> comments = new List<Comment>()
            {
                new Comment() { Id = 1, Content = "Kommentar", BlogpostId = 1, UserId = 3 }
            };

            List<Relation> relations = new List<Relation>()
            {
                new Relation() { Id = 1, FollowerId = 3, FollowedId = 1  },
                new Relation() { Id = 2, FollowerId = 3, FollowedId = 2  }
            };


            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Blogpost>().HasData(posts);
            modelBuilder.Entity<Comment>().HasData(comments);
            modelBuilder.Entity<Relation>().HasData(relations);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Blogpost> Blogposts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Relation> Relations { get; set; }


    }
}
