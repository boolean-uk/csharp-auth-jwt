using Microsoft.EntityFrameworkCore;

namespace exercise_auth_jwt.DatabaseContext
{
    public static class MigrationRunner
    {
        public static void ApplyProjectMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                //var migrationsApplied = db.Database.GetAppliedMigrations().Any();
                db.Database.Migrate();
            }

        }
    }
}
