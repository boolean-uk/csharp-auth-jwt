using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO.Requests
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<BlogAuthorDTO> Blogs { get; set; } = new List<BlogAuthorDTO>();
    }
}
