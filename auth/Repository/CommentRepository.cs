

using auth.Db;
using auth.Model;
using Microsoft.EntityFrameworkCore;

namespace auth.Repository {

    public class CommentRepository : ICommentRepository
    {
        private DatabaseContext _db;

        public CommentRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Comment> AddComment(string Text, string UserId)
        {
            var comment = new Comment { Text = Text, UserId = UserId};
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteComment(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
            }
        }

        public Task<List<Comment>> GetAdminComments()
        {
            return _db.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _db.Comments.FindAsync(id);
        }

        public async Task<List<Comment>> GetComments(string UserId)
        {
            return await _db.Comments
                        .Where(c => c.UserId == UserId)
                        .ToListAsync();
        }

        public async Task<Comment> UpdateComment(int id, Comment updatedComment)
        {
            var comment = await _db.Comments.FindAsync(id);
            if (comment != null)
            {
                comment.Text = updatedComment.Text;
                await _db.SaveChangesAsync();
            }
            return comment;
        }
    }
}