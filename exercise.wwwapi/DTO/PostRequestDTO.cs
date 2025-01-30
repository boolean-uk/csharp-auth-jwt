using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    [NotMapped]
    public class PostRequestDTO
    {
        public int AuthorId { get; set; }
        public string Text { get; set; }
    }
}
