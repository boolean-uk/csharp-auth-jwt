using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Models
{
    [Table("Post")]
    [PrimaryKey("postId")]
    public class Post
    {
        [Column("postId")]
        public int postId { get; set; }
        [Column("postTitle")]
        public string postTitle { get; set; }
        [Column("userId")]
        public int? userId { get; set; }
        [Column("user")]
        public virtual User user { get; set; }
        [Column("content")]
        public string content { get; set; }
        [NotMapped]
        public virtual List<Comment> comments { get; set; } = new List<Comment>();
    }
}
