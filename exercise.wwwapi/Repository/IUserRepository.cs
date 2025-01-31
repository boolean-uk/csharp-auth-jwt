using exercise.wwwapi.Model;

namespace exercise.wwwapi.Repository
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task FollowUser(int userId, int targetUserId);
        Task UnfollowUser(int userId, int targetUserId);
        Task<List<User>> GetFollowedUsers(int userId);
    }
}
