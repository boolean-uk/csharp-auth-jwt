using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository;

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        DbSet<T> Table { get; }

    }

