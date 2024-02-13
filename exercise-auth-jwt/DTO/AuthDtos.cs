using exercise_auth_jwt.Enum;

namespace exercise_auth_jwt.DTO
{
    public record RegisterDto(string Email, string Password);
    public record RegisterResponseDto(string Email, UserRole Role);

    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Token, string Email, UserRole Role);
}
