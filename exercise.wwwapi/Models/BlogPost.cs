using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blogsposts")]
    public class BlogPost
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [ForeignKey("author_id")]
        public int AuthorId { get; set; }

        public User Author { get; set; }
    }
}