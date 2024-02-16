using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.Data.Models
{
    [Table("posts")]
    public class Post : IEntity
    {
        [Key, Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("posted_at"), DataType("date")]
        public DateTime PostedAt { get; set; }
        [Column("updated_at"), DataType("date")]
        public DateTime UpdatedAt { get; set; }
        [Column("user_id"), ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
