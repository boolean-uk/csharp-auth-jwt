using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blog_posts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("author_id")]
        public string AuthorId { get; set; }
        [Column("text")]
        public string Text { get; set; }
        
    }
}
