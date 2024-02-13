using exercise_auth_jwt.DataModels;

namespace exercise_auth_jwt.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<BlogPost>> GetAllPosts();

        Task<BlogPost> MakeAPost(string text, string authorId);

        Task<BlogPost> EditPost(int postId, string text);
    }
}
