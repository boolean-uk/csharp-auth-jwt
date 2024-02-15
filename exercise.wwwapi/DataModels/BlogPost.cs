using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("blogs")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public required string Text { get; set; }
        [Column("author_id")]
        public required string AuthorId { get; set; }
    }
}
