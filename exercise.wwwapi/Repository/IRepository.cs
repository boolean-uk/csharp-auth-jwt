using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> configureQuery);
        Task<T> Insert(T entity);
        Task<IEnumerable<T>> InsertRange(IEnumerable<T> entities);
        Task<T> Update(T entity);
        Task<T> Delete(object id);
        Task Save();
        Task<T> GetById(params object[] keyValues);
        Task<T> GetById(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> GetById(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> configureQuery);
    }
}
