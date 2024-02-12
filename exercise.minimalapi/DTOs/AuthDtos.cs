using exercise.minimalapi.Enums;

namespace exercise.minimalapi.DTOs
{
    public record RegisterDto(string Email, string Password);
    public record RegisterResponseDto(string Email, UserRole Role);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Id, string Token, string Email, string Role);
}