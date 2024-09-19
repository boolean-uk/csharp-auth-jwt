using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace exercise.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DatabaseContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().Navigation(x => x.Comments).AutoInclude();
            modelBuilder.Entity<Comment>().Navigation(x => x.Post).AutoInclude();
            modelBuilder.Entity<Comment>().Navigation(x => x.User).AutoInclude();

        }

        public DbSet<User> Users { get; set; }       
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }


}
