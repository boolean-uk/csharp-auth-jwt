using exercise.wwwapi.DataModels;

namespace exercise.wwwapi.Data
{
    public class Seeder
    {
        public List<BlogPost> BlogPosts = new List<BlogPost>()
        {
            new BlogPost(){Id = 1, Text = "First blog post", AuthorId=""},
            new BlogPost(){Id = 2, Text = "Second blog post", AuthorId=""}
        };

    }
}
