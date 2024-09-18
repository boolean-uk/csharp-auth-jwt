
namespace exercise.wwwapi.Model
{
    public class BlogComment
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public string Text { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
