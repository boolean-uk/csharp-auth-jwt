

using auth.Db;
using auth.Model;

namespace auth.Repository {

    public class UserRepository : IUserRepository
    {
        private DatabaseContext _db;

        public UserRepository(DatabaseContext db)
        {
            _db = db;
        }

        public User? GetUser(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}