using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models;

public class UserFollow
{
    public int id { get; set; }
    public required int FollowerId { get; set; }

    [JsonIgnore]
    public virtual User? Follower { get; set; }
    public required int FolloweeId { get; set; }

    [JsonIgnore]
    public virtual User? Followee { get; set; }
}
