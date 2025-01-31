using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("comments")]
    public class Comment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [ForeignKey("blog_posts")]
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        [ForeignKey("users")]
        public string CommenterId { get; set; }
        public User Commenter { get; set; }
    }
}
