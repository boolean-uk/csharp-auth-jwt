using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class BlogRequestDTO
    {
        public string Text { get; set; }
    }
}
