using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Column("Text")]
        public string Text { get; set; }

        [Column("AuthorId")]
        public string AuthorId { get; set; }
        public List<string> Comments { get; set; }
    }
}
