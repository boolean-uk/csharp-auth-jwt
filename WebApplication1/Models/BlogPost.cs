using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("blog_posts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("author")]
        public string Author { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set;}
    }
}
