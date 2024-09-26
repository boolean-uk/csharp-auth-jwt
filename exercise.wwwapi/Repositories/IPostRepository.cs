using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repositories
{
    public interface IPostRepository
    {
        Task<BlogPost> Add(BlogPost blogPost);
        Task<ICollection<BlogPost>> GetAll();
    }
}
