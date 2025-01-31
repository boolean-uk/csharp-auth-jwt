using System.ComponentModel.DataAnnotations.Schema;
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

        public override User returnNewInstanceModel(params object[] pathargs)
        {
            return new User
            {
                Username = this.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(this.Password),
                Email = this.Email
            };
        }
    }
}
