using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class BlogPostRequestDto
    {

        public string Content { get; set; }

        public string UserId { get; set; }
    }
}
