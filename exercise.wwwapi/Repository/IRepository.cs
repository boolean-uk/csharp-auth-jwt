using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        Task<IEnumerable<T>> GetWithIncludes(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetWithNestedIncludes(params Func<IQueryable<T>, IQueryable<T>>[] includeActions);
        Task<T> GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        DbSet<T> Table { get; }

    }
}
