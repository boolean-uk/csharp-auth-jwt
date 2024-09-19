using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class BlogWithCommentsResponseDTO
    {
        public int PostId { get; set; }
        public string Username { get; set; }
        public string Post { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
