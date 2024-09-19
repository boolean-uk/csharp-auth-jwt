using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.BlogpostDTOs
{
    [NotMapped]
    public class BlogpostResponseDTO
    {
        public int Blogpostid { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
    }
}
