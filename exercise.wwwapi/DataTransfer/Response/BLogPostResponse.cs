namespace exercise.wwwapi.DataTransfer.Response
{
    public class BLogPostResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    
    }
}
