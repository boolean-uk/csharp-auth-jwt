using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels.Models
{
    [Table("blog_posts")]
    [PrimaryKey("Id")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("fk_user_id")]
        [ForeignKey("ApplicationUser")] 
        public string UserId { get; set; }
    }
}
