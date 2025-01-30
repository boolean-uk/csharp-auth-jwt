namespace exercise.wwwapi.DTO
{
    public class UserFollowerDTO
    {
        public int Id { get; set; }
        public UserDTO Follower { get; set; }
        public UserDTO Followee { get; set; }
    }
}
