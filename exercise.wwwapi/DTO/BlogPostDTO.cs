using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    [NotMapped]
    public class BlogPostDTO
    {
        public string Text { get; set; }
        public int AuthorId { get; set; }
    }
}