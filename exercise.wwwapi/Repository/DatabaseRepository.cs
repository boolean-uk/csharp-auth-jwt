using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
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
        public T GetById(int id)
        {
            return _table.Find(id);
        }

        public T Create(T entity)
        {
            _table.Add(entity);
            _db.SaveChangesAsync();
            return entity;
        }
        public T Update(T entity)
        {
            _table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public T Delete(int id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
            _db.SaveChangesAsync();
            return existing;
        }


        public void Save()
        {
            _db.SaveChanges();
        }
        public DbSet<T> Table { get { return _table; } }
    }
}
