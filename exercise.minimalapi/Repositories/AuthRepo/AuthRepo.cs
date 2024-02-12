using exercise.minimalapi.Data;
using exercise.minimalapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.minimalapi.Repositories.AuthRepo
{
    public class AuthRepo : IAuthRepo
    {
        BlogContext _db;
        public AuthRepo(BlogContext db) 
        {
            _db = db;
        }

        public async Task<ApplicationUser?> GetUserAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync( u=> u.Email == email);
        }
    }
}
