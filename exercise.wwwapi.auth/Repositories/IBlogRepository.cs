using exercise.wwwapi.auth.Models;

namespace exercise.wwwapi.auth.Repositories
{
    public interface IBlogRepository
    {

        Task<Blog> createBlog(string userId, string Author, string Title, string Description);
    }
}
