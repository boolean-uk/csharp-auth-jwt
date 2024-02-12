using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Models;
namespace exercise.wwwapi.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public UserDTO(Users user)
        {
            Id = user.Id;
            Name = user.UserName;
            Email = user.Email;
        }
    }
}