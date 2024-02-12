using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository
{
    public interface IBlogRepository
    {
        public Task<ICollection<BlogPost>> GetPosts();
    }
}
