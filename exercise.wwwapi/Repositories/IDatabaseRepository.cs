using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repositories
{
    public interface IDatabaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        Task<T> GetById(object id);
        Task Insert(T obj);
        Task Update(T obj);
        Task Delete(object id);
        Task Save();
        DbSet<T> Table { get; }
        /*
        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);
        public Task<T> Get(Expression<Func<T, bool>> predicate);
        */
    }
}
