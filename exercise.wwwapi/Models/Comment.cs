using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("comments")]
    public class Comment
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [ForeignKey("blogpostid")]
        public int BlogpostId { get; set; }

        [ForeignKey("userid")]
        public int UserId { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
        // Mapped data
        public User Author { get; set; }
        
    }
}
