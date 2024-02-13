using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DTO
{
    public record RegisterDTO(string Email, string Password);
    public record RegisterResponseDTO(string Email, UserRole Role);
    public record LoginDTO(string Email, string Password);
    public record AuthResponseDTO(string Token, string Email, UserRole Role);
}
