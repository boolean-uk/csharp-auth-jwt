using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class PostDTO
    {
 

        public string postTitle { get; set; }

        public int? userId { get; set; }
        
        public virtual string user { get; set; }
        public int? postId { get; set; }
      
        public string content { get; set; }

        public PostDTO(Post post)
        {
            postTitle = post.postTitle;
            userId = post.userId;
            postId = post.postId;
            content = post.content;
        }
    
}
}
