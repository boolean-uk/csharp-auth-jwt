using exercise.wwwapi.Model;
using System.Security.Claims;

namespace exercise.wwwapi.Repository
{
    public interface IRepository
    {
        public Task<List<BlogPost>> GetBlogPosts();
        public Task<BlogPost> UpdateBlogPost(int id, BlogPost data);
        public Task<BlogPost> DeleteBlogPost(int id);

        public Task<BlogPost> CreateBlog(BlogPost data);

    }
}
