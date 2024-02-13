using csharp_auth_jwt.Data;
using csharp_auth_jwt.Model;
using Microsoft.EntityFrameworkCore;

namespace csharp_auth_jwt.Repository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogPostContext _context;

        public BlogPostRepository(BlogPostContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPosts()
        {
            return await _context.BlogPosts.ToListAsync();
        }

        public async Task<BlogPost> GetBlogPost(int id)
        {
            return await _context.BlogPosts.FindAsync(id);
        }

        public async Task<BlogPost> AddBlogPost(BlogPost blogPost)
        {
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost> UpdateBlogPost(BlogPost blogPost)
        {
            _context.Entry(blogPost).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task DeleteBlogPost(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if(blogPost != null)
            {
                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
            }
        }
    }
}
