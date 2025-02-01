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
                    PasswordHash= BCrypt.Net.BCrypt.HashPassword("Tacos_with_pudding_and_burger_and_lassanga"),
                    Role = "User"},
                new User{ Id = 2,
                    Username = "Flurp",
                    Email = "flurp@bob.bob" ,
                    PasswordHash= BCrypt.Net.BCrypt.HashPassword("Oaks123"),
                    Role = "User"},
                new User{ Id = 3,
                    Username = "Glorp",
                    Email = "glorp@bob.bob" ,
                    PasswordHash= BCrypt.Net.BCrypt.HashPassword("glorps_password"), 
                    Role = "User"},
                new User{ Id = 4,
                    Username = "admin",
                    Email = "admin@admin.admin" ,
                    PasswordHash= BCrypt.Net.BCrypt.HashPassword("admin"),
                    Role = "Administrator"}
            };
            BlogPosts = new List<BlogPost>() { 
                new BlogPost{ Id = 1, AuthorId = 1, Text = "Bob hungry today..."},
                new BlogPost{ Id = 2, AuthorId = 2, Text = "Flurp not want tree, flurp has better things to do..."},
                new BlogPost{ Id = 3, AuthorId = 3, Text = "Where Glorp make password to use blog? Glorp got story to tell. Help"},
                new BlogPost{ Id = 4, AuthorId = 2, Text = "Ha Ha, Glorp don't know REST"},
                new BlogPost{ Id = 5, AuthorId = 3, Text = "Glorp ate rock once, Glorp hurt teeth"},
                new BlogPost{ Id = 6, AuthorId = 1, Text = "Maybe will try rock today, still hungry... "},
            };

            Comments = new List<Comment>() { 
                new Comment{ Id=1, BlogPostId=6, UserId=3,  Text = "No... Read my post. Glorp hurt tooth"},
                new Comment{ Id=2, BlogPostId=6, UserId=2,  Text = "Do it!"},
                new Comment{ Id=3, BlogPostId=6, UserId=3,  Text = "How do I login? need to tell Bob not to eat rock..."},
                new Comment{ Id=4, BlogPostId=5, UserId=1,  Text = "But was it a smooth or a jagged rock?"},
                new Comment{ Id=5, BlogPostId=5, UserId=3,  Text = "glorps_password"},
                new Comment{ Id=6, BlogPostId=5, UserId=3,  Text = "How do I remove?"},
                new Comment{ Id=7, BlogPostId=5, UserId=3,  Text = "loook at me, i'm dumb glorp. I eat rocks. haha. I am so stopid"},
                new Comment{ Id=8, BlogPostId=5, UserId=3,  Text = "I DID NOT DO THAT"},
             };
        }
        public List<BlogPost> BlogPosts { get; set; }
        public List<User> Users { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
