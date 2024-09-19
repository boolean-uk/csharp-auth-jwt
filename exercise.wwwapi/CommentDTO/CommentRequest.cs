using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.CommentDTO
{
    [NotMapped]
    public class CommentRequest
    {
        public string Comment { get; set; }
        public int PostId { get; set; }

    }
}
