using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_cinema_challenge.Models.Interfaces;

namespace exercise.wwwapi.Models
{
    [Table("blog_posts")]
    public class BlogPost :ICustomModel
    {
        [Key, Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int AuthorId {  get; set; }
        [Column("text")]
        public string Text {  get; set; }

        // Navigation properties 
        public User User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
