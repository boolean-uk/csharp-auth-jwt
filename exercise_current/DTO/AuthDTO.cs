using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DTO
{
    public record RegisterDTO(string Name, string Email, string Password);
    public record RegisterResponseDTO(string Name, string Email, UserRole Role);
    public record LoginDTO(string Name, string Email, string Password);
    public record AuthResponseDTO(string Name, string Token, string Email, UserRole Role);
}
