using exercise.minimalapi.Enums;
using exercise.minimalapi.Models;

namespace exercise.minimalapi.Repositories.AuthRepo
{
    public interface IAuthRepo
    {
        public Task<ApplicationUser?> GetUserAsync(string email);

        public Task<ApplicationUser> ChangeUserRoleAsync(string email, UserRole Role);
        public Task<List<ApplicationUser>> GetAllUsersAsync();
    }
}
