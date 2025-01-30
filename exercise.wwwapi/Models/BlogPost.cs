using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Models
{
    [Table("blogpost")]
    public class BlogPost
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [ForeignKey("user")]
        [Column("user_id")]
        public int UserId { get; set; }

        public User user { get; set; }
    }
}
