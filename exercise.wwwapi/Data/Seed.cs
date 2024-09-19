using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public static class Seed
    {
        public static async void SeedApi(this WebApplication app)
        {
            var db = new DatabaseContext();

            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new List<User>
                    {
                        new () { Username = "agmet", PasswordHash = "$2b$10$NB2Crurx3oPTJmOt7DO9xuXneErHa7Xt6A.vYivPDla.3c2FKX92K" },
                        new () { Username = "WizardNigel", PasswordHash = "$2b$10$Wlr6rOxNV0ry.DHBOnkl6eNPAzE//guLXu/fzzJJNSSCZ8Z5e7Q2O" },
                        new () { Username = "davetheames", PasswordHash = "$2b$10$/0Y2dXpakFs1dIzVWdH59O7AgR2svAo2bsIhR5OPxSQTZFrZ0OvwW"}
                    }
                );
                await db.SaveChangesAsync(); 
            }
            if (!db.Posts.Any())
            {
                var agron = await db.Users.FirstAsync(x => x.Username == "agmet");
                var nigel = await db.Users.FirstAsync(x => x.Username == "WizardNigel");
                var dave = await db.Users.FirstAsync(x => x.Username == "davetheames");
                
                List<Post> posts = [];

                Post agronPost = new()
                {
                    AuthorId = agron.Id,
                    Text = "Muay Thai is more than just a sport—it's a way of life. It teaches you to be disciplined, focused, and punching people in the face"
                };

                Post nigelPost = new()
                {
                    AuthorId = nigel.Id,
                    Text = "Here we go again, another 'React bro' telling me how it's the ultimate framework, solving all of our problems. Yeah, sure... Meanwhile, back in the real world, people are out here writing fast, simple C#! Keep chasing those JS frameworks, bros."
                };


                Post davePost = new()
                {
                    AuthorId = dave.Id,
                    Text = "You kids these days think everything is about the latest tech stack. Frameworks come and go, but fundamentals are forever. You can have all the fancy libraries in the world, but if you don’t understand algorithms and data structures, you’re just slapping together someone else’s code. Back in my day, we wrote efficient code because we *had* to. Every byte counted. Remember this: a solid foundation in computer science will always outlast trends in technology."
                };


                posts.Add( agronPost );
                posts.Add( nigelPost );
                posts.Add( davePost );

                db.Posts.AddRange( posts );
                await db.SaveChangesAsync();
            }
        }
    }
}
