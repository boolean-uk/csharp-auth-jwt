using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class BlogResponseDTO
    {
        public int PostId { get; set; }
        public string Username { get; set; }
        public string Post { get; set; }
    }
}
