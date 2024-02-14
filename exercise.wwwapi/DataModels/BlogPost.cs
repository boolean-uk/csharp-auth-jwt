using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("blogPosts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Text { get; set; }

        [Column("authorId")]
        public string AuthorId { get; set; }
    }
}
