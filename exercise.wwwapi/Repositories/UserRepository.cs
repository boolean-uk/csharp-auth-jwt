using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace exercise.wwwapi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _db;

        public UserRepository(DataContext db)
        {  
            _db = db; 
        }

        public async Task<User> GetUserByUsername(string username)
        {
           return await _db.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));
        }

        public async Task<User> Add(User user) 
        {
            await _db.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}
