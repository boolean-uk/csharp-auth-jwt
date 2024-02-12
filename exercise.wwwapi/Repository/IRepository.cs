using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<Blogpost>> GetPosts();
        Task<Blogpost> CreateBlogpost(Blogpost post);
        Task<Blogpost?> UpdateBlogPost(int blogpostId, Blogpost post);
        

    }
}
