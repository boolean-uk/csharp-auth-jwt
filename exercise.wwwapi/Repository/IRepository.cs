using System.Collections.Generic;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Add(T entity);
        T Update(T entity);
        void Save();

    }
}
