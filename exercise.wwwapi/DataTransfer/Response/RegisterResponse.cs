using exercise.wwwapi.Enum;
namespace exercise.wwwapi.DataTransfer.Response
{
    public class RegisterResponse
    {
        public string? Email { get; set; }
        public Role? UserRole { get; set; }

        public RegisterResponse(string email, Role? userRole)
        {
            Email = email;
            UserRole = userRole;
        }
    }
}
