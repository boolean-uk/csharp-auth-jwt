using WebApplication1.Enums;

namespace WebApplication1.Models.DTO
{
    public record RegisterDto(string Email, string Password);
    public record RegisterResponseDto(string Email, UserRole Role);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Token, string Email, UserRole Role);

}
