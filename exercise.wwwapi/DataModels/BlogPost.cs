namespace exercise.wwwapi.DataModels
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PostingUser { get; set; }
        public string AuthorId { get; set; }

        public BlogPost() { }
        public BlogPost(BlogPostRequset input) 
        {
            Text = input.Text;
        }
    }

    public class BlogPostRequset
    {
        public string Text { get; set; }
    }

    public class BlogPostPatch
    {
        public string Text { get; set; }
    }

    public class BlogPostGet
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PostingUser { get; set; }
        public string AuthorId { get; set; }
        public BlogPostGet(BlogPost input)
        {
            Id = input.Id; 
            Text = input.Text; 
            PostingUser = input.PostingUser; 
            AuthorId = input.AuthorId;
        }
    }
}
