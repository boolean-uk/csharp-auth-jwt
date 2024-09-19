using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("posts")]
    public class Post
    {
        [Column("id")] public int Id { get; set; }
        [Column("text")] public string Text { get; set; }
        [ForeignKey("User")]
        [Column("userId")] 
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
