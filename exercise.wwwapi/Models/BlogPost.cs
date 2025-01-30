using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class BlogPost : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }

        public Author Author { get; set; }
    }
}
