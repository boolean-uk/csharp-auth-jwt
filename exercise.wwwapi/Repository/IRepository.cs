using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        Task<T> GetById(object id);
        Task Insert(T obj);
        Task Update(T obj);
        Task Delete(object id);
        Task Save();
        DbSet<T> Table { get; }
    }
}
