using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DTO
{
    public record RegisterDTO(string username, string email, string password);
    public record RegisterResponseDTO(string username, string email, UserRole role);
}
