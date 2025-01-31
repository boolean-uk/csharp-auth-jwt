using System.Linq;
using System.Security.Claims;
using exercise.wwwapi.Data;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Model;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext _context;

        public BlogPostRepository(BlogDbContext context)
        {
            _context = context;
        }


        // Get all posts with their comments
        public async Task<List<BlogPost>> GetAllPostsAsync()
        {
            return await _context.BlogPosts.Include(b => b.Comments).ToListAsync();
        }

        // Create a new blog post
        public async Task<BlogPost> CreatePostAsync(BlogPost post)
        {
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        // Edit a post by its ID
        public async Task<BlogPost> EditPostAsync(int postId, BlogPost post)
        {
            var existingPost = await _context.BlogPosts.FindAsync(postId);
            if (existingPost == null)
                return null;

            existingPost.Text = post.Text;
            await _context.SaveChangesAsync();
            return existingPost;
        }

        // Get a post by its ID
        public async Task<BlogPost> GetPostByIdAsync(int postId)
        {
            return await _context.BlogPosts.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == postId);
        }

        // Add a comment to a post
        public async Task AddCommentToPost(int postId, Comment comment, ClaimsPrincipal user)
        {
            var post = await _context.BlogPosts.FindAsync(postId);
            if (post == null)
                throw new ArgumentException("Post not found");

            // Set the AuthorId from the logged-in user's Id
            comment.AuthorId = user.UserRealId().ToString(); // Assuming userId is stored in the claim "NameIdentifier"

            post.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
        // Get all comments for a post
        public async Task<List<Comment>> GetCommentsForPost(int postId)
        {
            var post = await _context.BlogPosts.Include(b => b.Comments)
                .FirstOrDefaultAsync(b => b.Id == postId);

            if (post == null)
                return new List<Comment>();

            return post.Comments;
        }

        // Get posts by a list of user IDs
        public async Task<List<BlogPost>> GetPostsByUsers(List<string> userIds)
        {
            return await _context.BlogPosts
                .Where(b => userIds.Contains(b.AuthorId))  // Now comparing strings
                .Include(b => b.Comments)
                .ToListAsync();
        }
    }

}
