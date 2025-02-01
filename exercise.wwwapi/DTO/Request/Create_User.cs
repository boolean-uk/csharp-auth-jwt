using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO.Request
{
    public class Create_User : DTO_Request_create<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public override User CreateAndReturnNewInstance(ClaimsPrincipal user, params object[] pathargs)
        {
            return new User
            {
                Username = this.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(this.Password),
                Email = this.Email,
                Role  = "User"
            };
        }

        protected override Func<IQueryable<User>, IQueryable<User>> GetEntryWithIncludes(User createdEntity, params object[] id)
        {
            return x => x.Where(x => x.Id == createdEntity.Id);
        }
    }
}
