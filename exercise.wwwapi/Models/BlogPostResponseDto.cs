using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class BlogPostResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
    }
}
