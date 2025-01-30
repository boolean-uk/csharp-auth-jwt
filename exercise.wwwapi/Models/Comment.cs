using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("comments")]
    public class Comment : BaseEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("blogpost_id")]
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
