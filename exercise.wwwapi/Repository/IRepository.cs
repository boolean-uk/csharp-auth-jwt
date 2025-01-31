using System.Linq.Expressions;

namespace exercise.wwwapi.Repository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
    Task<T?> Get(Expression<Func<T, bool>> predicate);
    Task<T?> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<T> Add(T entity);
    Task<T> Update(T entity);
}