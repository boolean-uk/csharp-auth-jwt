using Authentication.Model;

namespace Authentication.Repository
{
    public interface IRepository
    {
        public List<BlogPost> GetAllPosts();
        public BlogPost? GetPost(int id);
        public BlogPost CreatePost(string text, string authorId);
        public BlogPost UpdatePost(BlogPost post, string text);

        public ApplicationUser? GetUser(string email);
    }
}