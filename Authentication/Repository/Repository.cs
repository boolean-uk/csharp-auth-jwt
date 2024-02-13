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

        public BlogPost CreatePost(string title, string description, string author)
        {
            int id = 0;
            if (dataContext.Posts.Count()  > 0)
                id = dataContext.Posts.Last().Id;
            id++;

            var post = new BlogPost() { Id = id, Title = title, Description = description, Author = author };
            dataContext.Add(post);
            dataContext.SaveChanges();
            return post;
        }

        public BlogPost? UpdatePost(int id, string? title, string? description, string? author)
        {
            BlogPost? post = dataContext.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return null;

            post.Title = title;
            post.Description = description;
            post.Author = author;
            dataContext.SaveChanges();
            return post;
        }
    }
}