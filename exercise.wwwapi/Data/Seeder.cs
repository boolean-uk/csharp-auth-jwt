using exercise.wwwapi.Models;

namespace exercise.wwwapi.Data
{
    public class Seeder
    {
        private List<Guid> _authorIds = new List<Guid>
        {
            Guid.Parse("ae06628c-39d9-4315-a193-dc32f01c4c82"),
            Guid.Parse("06c8cc2d-b679-4b85-97e5-0ec1d5194b91"),
            Guid.Parse("9ddfeca9-4581-4033-8363-7b76a9590227"),
            Guid.Parse("ce0a9b28-5fb4-48d5-bdda-effd6ea843ea"),
            Guid.Parse("7b5f2a2b-72d7-4ed3-a8b6-c4a1b55182fe"),
            Guid.Parse("f7fd8480-1201-4d67-a11a-6b329b127483"),
            Guid.Parse("6558a75e-fe19-4af5-8d9a-17be7548f514"),
            Guid.Parse("82bbdd89-a252-4b94-86e2-fef200850fad"),
            Guid.Parse("cc2ab857-301d-423d-a712-55a1b364f574"),
            Guid.Parse("0ce13309-4f95-4b19-9360-ca233a0216e4")
        };
        private List<string> _firstNames = new List<string>()
        {
            "Alice", "Bob", "Charlie", "Diana", "Eve",
            "Frank", "Grace", "Hank", "Ivy", "Jack"
        };

        private List<string> _lastNames = new List<string>()
        {
            "Smith", "Johnson", "Williams", "Jones", "Brown",
            "Davis", "Miller", "Wilson", "Moore", "Taylor"
        };

        private List<string> _blogTitles = new List<string>()
        {
            "The Great Adventure", "Romance in Paris", "Space Odyssey",
            "The Last Stand", "Comedy Central", "Mystery Mansion",
            "Fast Wheels", "Horror Nights", "Dreams of Tomorrow", "The Epic Quest"
        };

        private List<string> _blogTexts = new List<string>()
        {
            "An exciting journey through uncharted lands.",
            "A love story set in the heart of Paris.",
            "Exploring the vastness of space and time.",
            "A thrilling tale of survival and courage.",
            "Laugh-out-loud moments from start to finish.",
            "A mysterious mansion with dark secrets.",
            "High-speed chases and adrenaline-pumping action.",
            "A spine-chilling tale of horror and suspense.",
            "Imagining a future full of possibilities.",
            "An epic quest to save the world."
        };

        private List<User> _users = new List<User>();
        private List<Author> _authors = new List<Author>();
        private List<BlogPost> _blogPosts = new List<BlogPost>();

        public Seeder()
        {
            DateTime staticDate = new DateTime(2023, 10, 1).ToUniversalTime();

            // Seed Users and Authors
            for (int i = 1; i <= 10; i++)
            {
                string firstName = _firstNames[(i - 1) % _firstNames.Count];
                string lastName = _lastNames[(i - 1) % _lastNames.Count];

                User user = new User
                {
                    Id = i,
                    Username = $"{firstName.ToLower()}.{lastName.ToLower()}",
                    PasswordHash = "$2a$12$WqH7F6m1H6Aqk3rLQ8d7J.5v4EFCB/Tx0q1fT/9rF0y8Df5y9XfWi", // static password
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}@example.com",
                    CreatedAt = staticDate,
                    UpdatedAt = staticDate
                };

                Author author = new Author
                {
                    Id = _authorIds[i - 1], // Use static GUIDs
                    DisplayName = $"{firstName} {lastName}",
                    Bio = $"A passionate writer with a love for {_blogTitles[(i - 1) % _blogTitles.Count]}.",
                    UserId = user.Id,
                    CreatedAt = staticDate,
                    UpdatedAt = staticDate
                };

                _users.Add(user);
                _authors.Add(author);
            }

            // Seed BlogPosts
            for (int i = 1; i <= 20; i++)
            {
                int authorIndex = (i - 1) % _authors.Count;
                BlogPost blogPost = new BlogPost
                {
                    Id = i,
                    Text = _blogTexts[(i - 1) % _blogTexts.Count],
                    AuthorId = _authors[authorIndex].Id,
                    CreatedAt = staticDate.AddDays(i),
                    UpdatedAt = staticDate.AddDays(i)
                };

                _blogPosts.Add(blogPost);
            }
        }

        public List<User> Users => _users;
        public List<Author> Authors => _authors;
        public List<BlogPost> BlogPosts => _blogPosts;
    }
}
