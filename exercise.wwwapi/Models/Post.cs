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
        [Column("content")]
        public string content { get; set; }
    }
}
