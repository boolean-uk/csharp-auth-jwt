using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string identification);
        Task<User> Add(User entity);
    }
}
