using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("blogposts")]
    public class Blogpost
    {
        [Column("blogpost_id")]
        public int id { get; set; }

        [Column("text")]
        public required string Text { get; set; }

        [Column("author_id")]
        public required string AuthorId { get; set; }
    }
}
