using exercise.wwwapi.Models.Enums;

namespace exercise.wwwapi.Models
{
    public class UserRelations
    {
        public int Id { get; set; }
        public string FollowerId { get; set; }
        public Author Follower { get; set; }
        public string FollowedId { get; set; }
        public Author Followed { get; set; }
        public Status Status { get; set; }
    }
}
