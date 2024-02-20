using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blog_posts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("author_id")]
        [ForeignKey(nameof(AppUser))]
        public string AuthorId { get; set; }
    }
}
