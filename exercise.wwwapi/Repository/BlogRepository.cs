using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class BlogRepository : IBlogRepository
    {
        DatabaseContext _db;

        public BlogRepository(DatabaseContext database)
        {
            _db = database;
        }

        public async Task<BlogPost> CreatePost(string author, string text)
        {
            BlogPost post = new BlogPost();
            post.Author = author;
            post.Text = text;
            await _db.Posts.AddAsync(post);
            _db.SaveChanges();
            return post;
        }

        public async Task<BlogPost> EditPost(BlogPost post, string text)
        {
            post.Text = text;
            await _db.SaveChangesAsync();
            return post;
        }

        public async Task<ICollection<BlogPost>> GetOwnPosts(string author_id)
        {
            List<BlogPost> results = await _db.Posts.Where(x => x.Author == author_id).ToListAsync();
            return results;
        }

        public async Task<BlogPost?> GetPostById(int id)
        {
            return await _db.Posts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<BlogPost>> GetPosts()
        {
            return await _db.Posts.ToListAsync();
        }
    }
}
