using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blog_posts")]
    public class BlogPost : BaseEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
