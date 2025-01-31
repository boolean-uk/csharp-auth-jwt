using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Models
{
    public class BlogPost
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [ForeignKey("user_id")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
