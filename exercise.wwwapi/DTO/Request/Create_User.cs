using System.ComponentModel.DataAnnotations.Schema;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO.Request
{
    public class Create_User : IDTO_Request_create<Create_User, User>
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public static Task<User?> create(IRepository<User> repo, Create_User dto, params object[] pathargs)
        {
            User u = new User
            {
                Username = dto.Username,
                PasswordHash = dto.PasswordHash,
                Email = dto.Email
            };
            return repo.CreateEntry(u);
        }


    }
}
