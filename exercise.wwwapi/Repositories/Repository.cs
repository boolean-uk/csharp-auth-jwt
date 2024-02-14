using exercise.wwwapi.Models;
using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;


namespace exercise.wwwapi.Repositories
{
    public class Repository : IRepository
    {
        DataContext _db;

        public Repository(DataContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Blogpost>> GetPostsAsync()
        {
            return await _db.Posts.Include(p => p.Author).ToListAsync();
        }

        public async Task<Blogpost?> CreatePost(string title, string text, int AuthorID)
        {
            if (title == "" || text == null) return null;
            //if _db.Authors.Contains(Author.Id == AuthorID);
            var post = new Blogpost { Title = title, Text = text, AuthorId = AuthorID };
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
            return post;
        }
    }
}
