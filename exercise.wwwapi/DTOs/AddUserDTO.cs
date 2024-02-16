using exercise.Data.Enums;

namespace exercise.wwwapi.DTOs
{
    public class AddUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
    }
}
