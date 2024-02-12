namespace exercise.wwwapi.DTO
{
    public class Payload
    {
        public record CreateApplicationUserPayload(string Name, string Email, string Password);
        public record LoginPayload(string Email, string Password);
        public record CreateBlogpostPayload(string Title, string Description);
    }
}
