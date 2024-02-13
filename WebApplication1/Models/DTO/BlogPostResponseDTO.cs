namespace WebApplication1.Models.DTO
{
    public class BlogPostResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }

        public BlogPostResponseDTO(BlogPost blogpost)
        {
            Id = blogpost.Id;
            Title = blogpost.Title;
            Text = blogpost.Text;
            Author = blogpost.Author;
        }
    }
}
