using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("user_follow")]
    public class UserFollow
    {
        [Column("follower_id")]
        public int FollowerId { get; set; }
        public User Follower { get; set; }

        [Column("followed_id")]
        public int FollowedId { get; set; }
        public User Followed { get; set; }
    }
}
