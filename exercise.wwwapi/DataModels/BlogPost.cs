using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("cars")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("author_id")]
        public string AuthorId { get; set; } // Assuming authorId is a string (e.g., UUID)

        [Column("created_date")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_date")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
