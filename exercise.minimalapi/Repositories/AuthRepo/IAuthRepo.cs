using exercise.minimalapi.Models;

namespace exercise.minimalapi.Repositories.AuthRepo
{
    public interface IAuthRepo
    {
        public Task<ApplicationUser?> GetUserAsync(string email);
    }
}
