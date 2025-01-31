using System.Security.Claims;
using exercise.wwwapi.Model;

namespace exercise.wwwapi.Repository
{
    public interface IBlogPostRepository
    {
        Task<List<BlogPost>> GetAllPostsAsync();
        Task<BlogPost> CreatePostAsync(BlogPost post);
        Task<BlogPost> EditPostAsync(int postId, BlogPost post);
        Task<BlogPost> GetPostByIdAsync(int postId);
        Task AddCommentToPost(int postId, Comment comment, ClaimsPrincipal user);
        Task<List<Comment>> GetCommentsForPost(int postId);
        Task<List<BlogPost>> GetPostsByUsers(List<string> userIds);
    }
}
