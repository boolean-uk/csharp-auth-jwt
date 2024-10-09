using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models.DTOs
{
    public class GetBlogPostDTO
    {
        public string Text { get; set; }
        public int AuthorId { get; set; }
    }

    [NotMapped]
    public class PostBlogPostDTO
    {
        public string Text { get; set; }
        public int AuthorId { get; set; }
    }

    [NotMapped]
    public class PutBlogPostDTO
    {
        public string Text { get; set; }
        public int AuthorId { get; set; }
    }
}
