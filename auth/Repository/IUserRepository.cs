
using auth.Model;

namespace auth.Repository
{
    public interface IUserRepository
    {
        public User? GetUser(string email);
    }
}