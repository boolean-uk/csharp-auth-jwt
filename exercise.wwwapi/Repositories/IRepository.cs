using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repositories
{
    public interface IRepository
    {

        public Task<IEnumerable<Blogpost>> GetPostsAsync();
        public Task<Blogpost?> CreatePost(string title, string text, int AuthorID);
    }
}
