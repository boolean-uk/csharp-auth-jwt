using Authentication.Enums;

namespace Authentication.DTO
{
    public record RegisterDto(string UserName, string Email, string Password);
    public record LoginDto(string UserName, string Email, string Password);
    public record AuthResponseDto(string Token, string UserName, string Email, UserRole Role);
}