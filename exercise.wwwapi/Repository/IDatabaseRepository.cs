﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IDatabaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Delete(object id1, object id2);
        void Save();
        DbSet<T> Table { get; }
    }
}
