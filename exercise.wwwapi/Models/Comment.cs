using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_cinema_challenge.Models.Interfaces;

namespace exercise.wwwapi.Models
{
    [Table("comments")]
    public class Comment : ICustomModel
    {
        [Key,Column("id")]
        public int Id { get; set; }
        [Column("blog_post_id")]
        public int BlogPostId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("text")]
        public string Text { get; set; }

        // Navigation properties 
        public BlogPost BlogPost { get; set; }
        public User User { get; set; }
    }
}
