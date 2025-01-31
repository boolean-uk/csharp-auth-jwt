using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blog_posts")]
    public class BlogPost
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [ForeignKey("users")]
        public string AuthorId { get; set; }
        public User Author { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
