using api_cinema_challenge.DTO.Interfaces;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.DTO.Response
{
    public class Get_User : DTO_Response<Get_User, User>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        protected override void _Initialize(User model)
        {
            Id = model.Id;
            Username = model.Username;
            PasswordHash = model.PasswordHash;
            Email = model.Email;

        }
    }
}
