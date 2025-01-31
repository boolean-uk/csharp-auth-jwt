using System.Linq;
using exercise.wwwapi.Data;
using exercise.wwwapi.Model;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogDbContext _context;

        private readonly IRepository<User> _repository;

        public UserRepository(BlogDbContext context, IRepository<User> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task FollowUser(int userId, int targetUserId)
        {
            var user = await _context.Users.FindAsync(userId);
            var targetUser = await _context.Users.FindAsync(targetUserId);

            if (user == null || targetUser == null)
                throw new ArgumentException("User not found");

            if (!user.Following.Contains(targetUser))
            {
                user.Following.Add(targetUser); // Add targetUser to Following list
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnfollowUser(int userId, int targetUserId)
        {
            var user = await _context.Users.FindAsync(userId);
            var targetUser = await _context.Users.FindAsync(targetUserId);

            if (user == null || targetUser == null)
                throw new ArgumentException("User not found");

            user.Following.Remove(targetUser);  // Remove targetUser from Following list
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetFollowedUsers(int userId)
        {
            var user = await _context.Users.Include(u => u.Following)
                                           .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new List<User>();

            return user.Following; // Return the actual list of User objects that the user is following
        }

    }
}
