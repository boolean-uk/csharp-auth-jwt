using exercise.wwwapi.DTOs.BlogPosts;

namespace exercise.wwwapi.DTOs.Author
{
    public class GetFollowingAuthorsPostsDTO
    {
        public string FollowedAuthor {  get; set; }
        public List<GetBlogPostDTO> BlogPosts { get; set; }
    }
}
