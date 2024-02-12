using BlogApplication.Data;
using BlogApplication.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BlogApplication.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private DatabaseContext _db;

        public BlogRepository(DatabaseContext db)
        {
            _db = db;
        }

        public List<BlogPost> GetAllBlogPosts(string userId, string role, string email)
        {

            var usr = _db.ApplicationUsers.FirstOrDefault(x => x.Email == email);

            if (role == "Admin") 
            {
                return _db.BlogPosts.ToList();
            }

            return _db.BlogPosts.Where(x => x.UserId == usr.Id).ToList();
        }

        public BlogPost AddBlogPost(string text, string userId, string email)
        {
            string newId = Guid.NewGuid().ToString();
            var usr = _db.ApplicationUsers.FirstOrDefault(x => x.Email == email);

            var blogPost = new BlogPost() { Id = newId, Text = text, UserId = usr.Id.ToString() };
            _db.Add(blogPost);
            _db.SaveChanges();
            return blogPost;
        }

        public BlogPost? GetBlogPost(string id)
        {
            return _db.BlogPosts.FirstOrDefault(t => t.Id == id);
        }


        public BlogPost? UpdateBlogPost(string id, BlogPostUpdatePayload updateData)
        {
            // check if task exists
            var post = GetBlogPost(id);
            if (post == null)
            {
                return null;
            }

            bool hasUpdate = false;

            if (updateData.Text != null)
            {
                post.Text = (string)updateData.Text;
                hasUpdate = true;
            }

            if (!hasUpdate) throw new Exception("No task update data provided");

            _db.SaveChanges();

            return post;
        }


        public ApplicationUser? GetUser(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

    }
}