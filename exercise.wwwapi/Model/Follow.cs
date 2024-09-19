namespace exercise.wwwapi.Model
{
    public class Follow
    {
        public string FollowerId { get; set; }
        public Author Follower { get; set; }
        public string FollowingId { get; set; }
        public Author Following { get; set; }
    }
}
