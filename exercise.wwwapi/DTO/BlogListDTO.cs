using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    [NotMapped]
    public class BlogListDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int AuthorId { get; set; }
    }
    
}
