using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.Models;
using exercise.wwwapi.ViewModels;
using Microsoft.EntityFrameworkCore.Update.Internal;

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

        public static void Update(this Post post, PostUpdate postUpdate)
        {
            if(!string.IsNullOrEmpty(postUpdate.Text)) post.Text = postUpdate.Text;
            if(postUpdate.UserId != post.UserId) post.UserId = postUpdate.UserId;
        }
    }
}
