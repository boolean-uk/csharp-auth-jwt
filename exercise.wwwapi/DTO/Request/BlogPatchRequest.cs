using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO.Request
{
    public class BlogPatchRequest
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Author { get; set; }
        public DateTime? createdAt { get; set; }
    }
}
