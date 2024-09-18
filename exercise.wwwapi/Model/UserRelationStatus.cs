using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    public enum Status { FOLLOW, FOLLOWING, UNNFOLLOW }

    public class UserRelationStatus
    {
        public int Id {  get; set; }
        public int FollowerId { get; set; }
        public Author Follower { get; set; }
        public int FollowedId { get; set; }
        public Author Followed { get; set; }
        public Status Status { get; set; }
    }
}
