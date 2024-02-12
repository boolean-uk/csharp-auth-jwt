using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DTO
{
    public class DTO
    {
        public record CreateUserDTO(string Email, UserRole Role);
        public record AuthResponseDTO(string Token, string Email, UserRole role);
    }
}
