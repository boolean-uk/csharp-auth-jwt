using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class BlogPostUpdateRequestDto
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public string? UserId { get; set; }
    }
}
