using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public enum Status { FOLLOW, FOLLOWING, UNFOLLOW, REQUESTED }
    public class RelationStatus
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Follower")]
        public int FollowerId { get; set; }

        public User Follower { get; set; }
        [ForeignKey("Followed")]
        public int FollowedId { get; set; }
        public User Followed { get; set; }

        public Status Status { get; set; }
    }
}
