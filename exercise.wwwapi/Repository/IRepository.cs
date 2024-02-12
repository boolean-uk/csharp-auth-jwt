using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository
{
    public interface IRepository
    {
        public Task<Posts> CreatePost(string text, string userId);
        public Task<Posts?> UpdatePost(int id, string text);
        public Task<Posts?> GetPost(int id);
        public Task<IEnumerable<Posts>> GetPosts();

    }
}
