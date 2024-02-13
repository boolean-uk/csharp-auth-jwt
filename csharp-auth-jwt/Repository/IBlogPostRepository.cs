using csharp_auth_jwt.Model;

namespace csharp_auth_jwt.Repository
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetBlogPosts();
        Task<BlogPost> GetBlogPost(int id);
        Task<BlogPost> AddBlogPost(BlogPost blogPost);
        Task<BlogPost> UpdateBlogPost(BlogPost blogPost);
        Task DeleteBlogPost(int id);
    }
}
