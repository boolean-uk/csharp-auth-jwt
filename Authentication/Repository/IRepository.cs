using Authentication.Model;

namespace Authentication.Repository
{
    public interface IRepository
    {
        public List<BlogPost> GetAllPosts();
        public BlogPost? GetPost(int id);
        public BlogPost CreatePost(string title, string description, string author);
        public BlogPost? UpdatePost(int id, string? title, string? description, string? author);
    }
}