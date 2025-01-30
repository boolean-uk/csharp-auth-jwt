using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class UserFollower
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FollowerId { get; set; } 

        [Required]
        public int FolloweeId { get; set; } 

        // Navigation properties
        [ForeignKey("FollowerId")]
        public User Follower { get; set; }

        [ForeignKey("FolloweeId")]
        public User Followee { get; set; }
    }
}
