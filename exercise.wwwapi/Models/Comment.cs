using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Models
{
    [Table("Comment")]
    [PrimaryKey("commentId")]
    public class Comment
    {
        [Column("commentId")]
        public int commentId {  get; set; }
        [Column("userId")]
        public int? userId { get; set; }
        [Column("content")]
        public string content {  get; set; }
        [Column("postId")]
        public int postId { get; set; }
        [NotMapped]
        public virtual Post post { get; set; }
        [NotMapped]
        public virtual User user { get; set; }
    }
}
