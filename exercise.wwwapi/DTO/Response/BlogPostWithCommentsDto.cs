namespace exercise.wwwapi.DTO.Response
{
    public class BlogPostWithCommentsDto
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public List<CommentResponseDto> Comments { get; set; }
    }
}
