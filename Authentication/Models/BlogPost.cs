using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Model
{
    [Table("blog_posts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("author")]
        public string Author { get; set; }
    }
}