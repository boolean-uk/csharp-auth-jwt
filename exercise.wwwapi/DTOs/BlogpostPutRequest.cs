using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class BlogpostPutRequest
    {
        public int? BlogpostId { get; set; }
        public string? Text { get; set; }
        public int? AuthorId { get; set; }
    }
}
