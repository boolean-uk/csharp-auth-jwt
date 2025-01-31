using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

public class UserFollow
{
    public int id { get; set; }
    public required int FollowerId { get; set; }
    public virtual User? Follower { get; set; }
    public required int FolloweeId { get; set; }
    public virtual User? Followee { get; set; }
}
