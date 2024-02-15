using exercise.wwwapi.DataModels;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace exercise.wwwapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure additional entity mappings and relationships here
            modelBuilder.Entity<Blogpost>().ToTable("blogposts").HasKey(b => b.id);
            base.OnModelCreating(modelBuilder); //  default Identity configurations

        }

        public DbSet<Blogpost> Blogposts { get; set; }

    }
}
