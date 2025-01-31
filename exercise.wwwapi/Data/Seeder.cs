using exercise.wwwapi.Models;

namespace exercise.wwwapi.Data
{
    public class Seeder
    {
        public Seeder()
        {
            Users = new List<User>()
            { 
                new User{ Id = 1,
                    Username = "Bob",
                    Email = "Bob@bob.bob"     ,
                    PasswordHash= BCrypt.Net.BCrypt.HashPassword("Tacos_with_pudding_and_burger_and_lassanga")},
                new User{ Id = 2,
                    Username = "Flurp",
                    Email = "flurp@bob.bob" ,
                    PasswordHash= BCrypt.Net.BCrypt.HashPassword("Oaks123")},
                new User{ Id = 3,
                    Username = "Glorp",
                    Email = "glorp@bob.bob" ,
                    PasswordHash= BCrypt.Net.BCrypt.HashPassword("Glorps first post!")}
            };
            BlogPosts = new List<BlogPost>() { 
                new BlogPost{ Id = 1, AuthorId = 1, Text = "Bob hungry today..."},
                new BlogPost{ Id = 2, AuthorId = 2, Text = "Flurp not want tree, flurp has better things to do..."},
                new BlogPost{ Id = 3, AuthorId = 3, Text = "Where Glorp make password to use blog? Glorp got story to tell. Help"},
                new BlogPost{ Id = 4, AuthorId = 2, Text = "Ha Ha, Glorp don't know REST"},
                new BlogPost{ Id = 5, AuthorId = 3, Text = "Glorp ate rock once, Glorp hurt teeth"},
                new BlogPost{ Id = 6, AuthorId = 1, Text = "Maybe will try rock today, still hungry... "},
            };
        }
        public List<BlogPost> BlogPosts { get; set; }
        public List<User> Users { get; set; }
    }
}
