using exercise.wwwapi.Models;
using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DatabaseContext _db;

        public Repository(DatabaseContext _databaseContext)
        {
            _db = _databaseContext;

        }
        public async Task<Posts> CreatePost(string text, string userid)
        {
            Posts Post = new Posts()
            {
                Text = text,
                UserId = userid
            };
            _db.Posts.Add(Post);
            await _db.SaveChangesAsync();
            return Post;
        }

        public async Task<Posts?> GetPost(int id)
        {
            Posts Post = await _db.Posts.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (Post == null)
            {
                return null;
            }
            return Post;
        }

        public async Task<IEnumerable<Posts>> GetPosts()
        {
             return await _db.Posts.Include(p => p.User).ToListAsync();
        }

        public async Task<Posts?> UpdatePost(int id, string text)
        {
            Posts? post = await GetPost(id);
            if (post != null)
            {
                post.Text = text;
            }
            await _db.SaveChangesAsync();
            return post;
        }
    }
}
