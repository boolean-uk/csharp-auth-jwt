using auth.exercise.Enums;

namespace auth.exercise.DTO
{
  public record RegisterDto(string Email, string Password);
  public record LoginDto(string Email, string Password);
  public record AuthResponseDto(string Token, string Email, Roles Role);
}