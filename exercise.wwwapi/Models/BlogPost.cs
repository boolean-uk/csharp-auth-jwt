namespace exercise.wwwapi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public Author Author { get; set; }
        public string Text { get; set; }
        public DateOnly Posted { get; set; }
        public ICollection<BlogComments> BlogComments { get; set; } = new List<BlogComments>();

    }
}
