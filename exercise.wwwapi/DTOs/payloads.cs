namespace exercise.wwwapi.DTOs
{
    public class payloads
    {
        public record CreatePostPayload(string Title, string Text, int AuthorID);
    }
}
