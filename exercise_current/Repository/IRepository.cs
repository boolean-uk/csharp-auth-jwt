using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<Posts>> GetAllPosts();
        Task<Posts?> GetPost(int id);
        Task<Posts> CreatePost(string Text, int AuthorId);
        Task<Posts?> UpdatePost(int id, string Text, int AuthorId);
        Task<User?> GetUser(int id);
    }
}
