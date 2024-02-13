using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository
{
    public interface IBlogRepository
    {
        public Task<ICollection<BlogPost>> GetPosts();
        public Task<BlogPost?> GetPostById(int id);
        public Task<BlogPost> CreatePost(string author, string text);
        public Task<ICollection<BlogPost>> GetOwnPosts(string author_id);
        public Task<BlogPost> EditPost(BlogPost post, string text);
    }
}
