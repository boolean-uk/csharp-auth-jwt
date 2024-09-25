using exercise.wwwapi.Models.Enums;

namespace exercise.wwwapi.DTO.GetResponses
{
    public class GetUserRelationsResponse
    {
        public int Id { get; set; }
        public string FollowerName { get; set; }
        public string FollowedName { get; set; }
        public Status Status { get; set; }
    }
}
