using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(object id);
        Task<T?> Insert(T obj);
        Task<T?> Update<U>(IMapper mapper, object id, U source);
        Task<T?> Delete(object id);
        Task<T?> GetBy(Expression<Func<T, bool>> pred);
        Task<bool> Exists(object id);
    }
}
