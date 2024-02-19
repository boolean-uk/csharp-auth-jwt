using System.ComponentModel.DataAnnotations.Schema;

namespace workshop.webapi.DataModels
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
        [ForeignKey(nameof(ApplicationUser))]
        public string AuthorId { get; set; }
    }
}
