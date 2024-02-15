using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace exercise.wwwapi.DataModels
{
    [Table("blogPosts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }

        [ForeignKey("Author")]
        [Column("authorId")]
        public string AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
