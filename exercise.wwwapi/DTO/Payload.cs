using exercise.wwwapi.Enum;

namespace exercise.wwwapi.DTO
{

    public record RegisterDto(string Email, string Password);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Token, string Email, Roles Role);

    public record BlogPostPayload(string Title, string Text, string Author, DateTime DateTime);

    public record BlogPutPayload(string Title, string Text, string Author, DateTime DateTime);

}
