using exercise.wwwapi.Models;
namespace exercise.wwwapi.DTO
{
    public record RegisterDto(string Email, string Password); // dont let api req set role. That should be done manually by an admin. Super admin is seeded from the start
    public record RegisterResponseDto(string Email, UserRole
    Role);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Token, string Email,
    UserRole Role); // send back generated token, user email and let application know which role user has
}