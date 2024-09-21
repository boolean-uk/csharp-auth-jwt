using exercise.wwwapi.Models;

public class AuthorFollower
{
    public string FollowerId { get; set; }  
    public Author Follower { get; set; }

    public string FollowedId { get; set; }
    public Author Followed { get; set; }
}