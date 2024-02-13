using exercise.minimalapi.Models;

namespace exercise.minimalapi.Repositories
{
    public interface IPostsRepo
    {
        public Task<Post> Add(string text, string authorId);
        public Task<ICollection<Post>> GetAll();
        public Task<ICollection<Post>> GetPostsByUserId(string userId);
    }
}
