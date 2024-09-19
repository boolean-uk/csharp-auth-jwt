namespace exercise.wwwapi.Models.DataTransferObjects.RequestDTO
{
    public class CommentRequestDTO
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int BlogpostId { get; set; }
    }
}