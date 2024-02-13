using exercise.minimalapi.Data;
using exercise.minimalapi.Enums;
using exercise.minimalapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace exercise.minimalapi.Repositories.AuthRepo
{
    public class AuthRepo : IAuthRepo
    {
        private BlogContext _db;
        private UserManager<ApplicationUser> _userManager;
        public AuthRepo(BlogContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> ChangeUserRoleAsync(string email, UserRole Role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { throw new ArgumentNullException("User not found"); }

            user.Role = Role;

            await _userManager.UpdateAsync(user);
            return user;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
