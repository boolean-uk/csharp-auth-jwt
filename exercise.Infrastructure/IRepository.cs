using exercise.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.Infrastructure
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> Get(string id);
        Task<List<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Delete(string id);
        Task<T> Update(T entity);
    }
}
