using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public List<FollowerDTO> Followers { get; set; } = new List<FollowerDTO>();
    }
}
