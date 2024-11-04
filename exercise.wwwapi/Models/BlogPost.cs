using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blogposts")]
    public class BlogPost
    {
        [Key]
        [Column("id")]

        public int Id { get; set; }

        [Column("text")]

        public string Text { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

    }
}
