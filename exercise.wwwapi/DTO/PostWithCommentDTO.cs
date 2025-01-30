using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTO
{
    public class PostWithCommentDTO
    {
        public string postUsername {  get; set; }
        public string postText {  get; set; }
        public int PostId { get; set; }
        public List<CommentWithUser>comments { get; set; } = new List<CommentWithUser>();
    }
}
