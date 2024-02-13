using Authentication.Data;
using Authentication.Model;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Repository
{
    public class Repository : IRepository
    {
        private DataContext dataContext;

        public Repository(DataContext db)
        {
            dataContext = db;
        }

        public List<BlogPost> GetAllPosts()
        {
            return dataContext.Posts.ToList();
        }

        public BlogPost? GetPost(int id)
        {
            return dataContext.Posts.FirstOrDefault(p => p.Id == id);
        }

        public BlogPost CreatePost(string text, string authorId)
        {
            int id = 0;
            if (dataContext.Posts.Count() > 0)
                id = dataContext.Posts.ToList().Last().Id;
            id++;

            var post = new BlogPost() { Id = id, Text = text, AuthorId = authorId };
            dataContext.Add(post);
            dataContext.SaveChanges();
            return post;
        }

        public BlogPost UpdatePost(BlogPost post, string text)
        {
            post.Text = text;
            dataContext.SaveChanges();
            return post;
        }

        public ApplicationUser? GetUser(string email)
        {
            return dataContext.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}