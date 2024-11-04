using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IDatabaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        T GetById(int id);
        T Create(T entity);
        T Update(T entity);
        T Delete(int id);
        void Save();
        DbSet<T> Table { get; }
    }
}
