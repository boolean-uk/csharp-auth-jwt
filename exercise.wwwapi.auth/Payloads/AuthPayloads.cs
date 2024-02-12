using exercise.wwwapi.auth.Enums;

namespace exercise.wwwapi.auth.Payloads
{
    public class AuthPayloads
    {
        public record RegisterPayload(string Username, string Email, string Password);
        public record RegisterResPayload(string Email, UserRole Role);
        public record LoginPayload(string Email, string Password);
        public record LoginResPayload(string Token, string Email, UserRole Role);
    }
}
