using exercise.wwwapi.DTO.GetModels;

namespace exercise.wwwapi.DTO.GetResponses
{
    public class GetFollowingAuthorResponse
    {
        public string FollowedAuthor { get; set; }
        public List<GetBlogPostResponses> BlogPosts { get; set; }
    }
}
