
using exercise.wwwapi.auth.Data;
using exercise.wwwapi.auth.Models;

namespace exercise.wwwapi.auth.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private DatabaseContext _db;

        public BlogRepository(DatabaseContext db)
        {
            _db = db;
        }


        public async Task<Blog> createBlog(string user_id, string author, string title, string description)
        {
            var result = new Blog() { AuthorId = user_id, AuthorName = author, Title = title, Description = description, CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow };
            _db.Blogs.Add(result);
            await _db.SaveChangesAsync();

            return result;
            throw new NotImplementedException();
        }
    }
}
