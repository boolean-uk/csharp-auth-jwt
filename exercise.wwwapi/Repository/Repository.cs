using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

using exercise.wwwapi.Data;

namespace exercise.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DataContext _db;
        public Repository(DataContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _db.Posts.ToListAsync();
        }

        public async Task<Post> GetPost(int id)
        {
            return await _db.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> AddPost(Post post)
        {
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
            return post;
        }

        public async Task<Post> UpdatePost(Post post)
        {
            _db.Entry(post).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return post;
        }
    }
}
