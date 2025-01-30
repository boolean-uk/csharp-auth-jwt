namespace exercise.wwwapi.Models
{
    public class UserRelation
    {
        public int FromFollowId { get; set; }
        public User FromFollow { get; set; }
        public int ToFollowId { get; set; }
        public User ToFollow { get; set; }
    }
}
