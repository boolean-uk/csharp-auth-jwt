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
            return blogPost;
        }

        public async Task<ICollection<BlogPost>> GetAll()
        {
            return await _db.BlogPosts.Include(b => b.Author).ToListAsync();
        }

        //public async Task<ICollection<BlogPost>> GetByString(string authorId)
        //{
            //return await _db.BlogPosts.Where(b => b.authorId.Equals(authorId)).ToListAsync();
        //}
    }
}
