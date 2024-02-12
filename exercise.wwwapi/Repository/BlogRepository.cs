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
        public async Task<ICollection<BlogPost>> GetPosts()
        {
            return await _db.Posts.ToListAsync();
        }
    }
}
