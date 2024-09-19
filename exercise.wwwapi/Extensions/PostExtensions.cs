using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Extensions
{
    public static class PostExtensions
    {
        public static PostDTO ToDTO(this Post post)
        {
            return new PostDTO()
            {
                Id = post.Id,
                Text = post.Text,
                Author = post.User.Username
            };
        }
    }
}
