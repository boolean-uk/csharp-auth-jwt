using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("relations")]
    public class Relation
    {
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("followerid")]
        public int FollowerId { get; set; }

        [ForeignKey("followedid")]
        public int FollowedId { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
        // Mapped data
        public User Follower { get; set; }
        public User Followed { get; set; }
    }
}
