using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blogposts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("authorid")]
        public string AuthorId { get; set; }
    }
}
