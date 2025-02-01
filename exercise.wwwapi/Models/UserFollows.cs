using System.ComponentModel.DataAnnotations.Schema;
using api_cinema_challenge.Models.Interfaces;

namespace exercise.wwwapi.Models
{
    [Table("user_follows")]
    public class UserFollows : ICustomModel
    {
        [Column("user_who_follows_id")]
        public int FollowerId { get; set; }
        [Column("user_to_follow_id")]
        public int FollowingsId { get; set; }

        // Navigation properties 
        public User Follower { get; set; }
        public User Following { get; set; }
    }
}
