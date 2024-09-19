using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("blogposts")]
    public class Blogpost
    {
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("authorid")]
        public int AuthorId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedat")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        // Mapped data
        public User Author { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
