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

        public async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includeExpressions)
        {
            if (includeExpressions.Any())
            {
                var set = includeExpressions
                    .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                     (_table, (current, expression) => current.Include(expression));
            }
            return await _table.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }
        public async Task<T> GetById(object id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T> Reload(T obj)
        {
            await _db.Entry(obj).ReloadAsync();
            return obj;
        }

        public async Task<T> Insert(T obj)
        {
            _table.Add(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
        public async Task<T> Update(T obj)
        {
            _table.Attach(obj);
            _db.Entry(obj).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<T> Delete(object id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
            await _db.SaveChangesAsync();
            return existing;
        }

        public DbSet<T> Table { get { return _table; } }

    }
}