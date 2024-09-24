using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    [Table("blogposts")]
    public class BlogPost
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [ForeignKey("users")]
        public int AuthorID { get; set; }
        public User Author { get; set; }
    }
}
