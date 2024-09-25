using exercise.wwwapi.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class BlogPostDTO
    {
        public string Text { get; set; }
        public int AuthorID { get; set; }
    }
}