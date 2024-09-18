using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class BlogpostRequestDTO
    {
        public string Text { get; set; }

    }
}
