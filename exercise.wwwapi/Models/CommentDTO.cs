using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class CommentDTO
    {
        public string Username { get; set; }
        public string Text { get; set; }
    }
}
