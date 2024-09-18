using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class BlogpostResponseDTO
    {
        public int Blogpostid { get; set; }
        public string Text { get; set; }
        public string Author {  get; set; } 
    }
}
