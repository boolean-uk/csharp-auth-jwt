namespace exercise.wwwapi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
        public string AuthorId { get; set; }

        public BlogPost() { }
        public BlogPost(PostBlogPost post)
        {
            Text = post.Text;
        }

        public BlogPost(PutBlogPost post)
        {
            Text = post.Text ?? Text!;
        }
    }

    public class BlogPostDto
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Text { get; set; }

        public BlogPostDto(BlogPost blog) 
        { 
            Id = blog.Id;
            Text = blog.Text;
            AuthorId = blog.AuthorId;
        }
    }



    public class PostBlogPost
    {
        public string Text { get; set; }
    }

    public class PutBlogPost
    {
        public string? Text { get; set; }
        public string? AuthorId { get; set; }
    }
}
