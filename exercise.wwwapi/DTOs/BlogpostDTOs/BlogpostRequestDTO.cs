using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.BlogpostDTOs
{
    [NotMapped]
    public class BlogpostRequestDTO
    {
        public string Text { get; set; }

    }
}
