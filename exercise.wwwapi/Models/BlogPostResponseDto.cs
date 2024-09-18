using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class BlogPostResponseDto
    {
        public string Content { get; set; }

    }
}
