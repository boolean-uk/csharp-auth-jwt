using exercise.wwwapi.auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.auth.Data
{
    public class DatabaseContext : IdentityUserContext<ApplicationUser>
    {
        private readonly string _connectionstring;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionstring = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }

        public override void OnModelCreating(ModelBuilder modelBuilder) {




            base.OnModelCreating(modelBuilder);
        }


    }
}
