using exercise.wwwapi.DataModels;

namespace exercise.wwwapi.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<BlogPost>> GetAllPosts();

        Task<BlogPost> MakeAPost(string text, string authorId);

        Task<BlogPost> EditPost(string postId, string text, string authorId);
    }
}
