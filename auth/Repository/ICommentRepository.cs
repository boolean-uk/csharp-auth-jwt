using auth.Model;
using System.Collections.Generic;

namespace auth.Repository
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetComments(string UserId);

        public Task<Comment> GetCommentById(int id);

        public Task<Comment> AddComment(string Text, string UserId);

        public Task<Comment> UpdateComment(int id, Comment updatedComment);

        public Task DeleteComment(int id);
        Task<List<Comment>> GetAdminComments();
    }
}