using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DataTransfer.Requests
{
    public class RegistrationRequest
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required Role Role { get; set; }
    }
}
