using BlogApplication.Models;

namespace BlogApplication.Repository
{
    public interface IBlogRepository
    {
        public List<BlogPost> GetAllBlogPosts(string id, string role, string email);

        public BlogPost? GetBlogPost(string id);

        public BlogPost AddBlogPost(string text, string authId, string email);

        public BlogPost? UpdateBlogPost(string id, BlogPostUpdatePayload updateData);

        public ApplicationUser? GetUser(string email);
    }
}
