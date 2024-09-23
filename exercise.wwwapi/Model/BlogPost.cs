using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    [Table("blogposts")]
    public class BlogPost
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [ForeignKey("users")]
        public string AuthorID { get; set; }
        public User Author { get; set; }
    }
}
