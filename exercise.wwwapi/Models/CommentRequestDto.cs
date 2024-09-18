using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class CommentRequestDto
    {
            public string Content { get; set; }
            public string PostId { get; set; }

        
    }
}
