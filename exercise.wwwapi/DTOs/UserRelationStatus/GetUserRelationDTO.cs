using exercise.wwwapi.Model;

namespace exercise.wwwapi.DTOs.UserRelationStatus
{
    public class GetUserRelationDTO
    {
        public int Id { get; set; }
        public string FollowerName { get; set; }
        public string FollowedName { get; set; }
        public Status Status { get; set; }
    }
}
