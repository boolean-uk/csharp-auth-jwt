using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DTO
{
    public record LoginDTO(string username, string password);
    public record LoginResponseDTO(string Token, string username, UserRole role);
}
