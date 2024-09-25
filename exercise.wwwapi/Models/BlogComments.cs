namespace exercise.wwwapi.Models
{
    public class BlogComments
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public Author Author { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public string Text { get; set; }
        public DateTime CommentTime { get; set; }
    
}
}
