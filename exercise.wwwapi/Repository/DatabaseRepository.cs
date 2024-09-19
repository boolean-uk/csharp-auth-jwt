﻿using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : class
    {

        private DatabaseContext _db;
        private DbSet<T> _table = null;

        public DatabaseRepository()
        {
            _db = new DatabaseContext();
            _table = _db.Set<T>();
        }
        public DatabaseRepository(DatabaseContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions)
        {
            if (includeExpressions.Any())
            {
                var set = includeExpressions
                    .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                     (_table, (current, expression) => current.Include(expression));
            }
            return _table.ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return _table.ToList();
        }
        public T GetById(object id)
        {
            return _table.Find(id);
        }

        public void Insert(T obj)
        {
            _table.Add(obj);
        }
        public void Update(T obj)
        {
            _table.Attach(obj);
            _db.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
        }

        public void Delete(object id1, object id2)
        {
            T existing = _table.Find(id1, id2);
            _table.Remove(existing);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public DbSet<T> Table { get { return _table; } }

    }
}
