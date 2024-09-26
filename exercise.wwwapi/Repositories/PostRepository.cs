using System.Collections;
using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace exercise.wwwapi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private DataContext _db;

        public PostRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<BlogPost> Add(BlogPost blogPost)
        {
            await _db.AddAsync(blogPost);
            await _db.SaveChangesAsync();
            await _db.Entry(blogPost).Reference(b => b.Author).LoadAsync();
            return blogPost;
        }

        public async Task<ICollection<BlogPost>> GetAll()
        {
            return await _db.BlogPosts.Include(b => b.Author).ToListAsync();
        }

    }
}
