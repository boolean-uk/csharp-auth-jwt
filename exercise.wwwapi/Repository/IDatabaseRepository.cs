using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IDatabaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        Task<T> GetById(object id);
        Task<T> Insert(T obj);
        Task<T> Reload(T obj);
        Task<T> Update(T obj);
        Task<T> Delete(object id);
        DbSet<T> Table { get; }

    }
}