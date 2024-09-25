using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DataBaseContext _db;
        private DbSet<T> _table;
        public Repository(DataBaseContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }


        public async Task Delete(object id)
        {
            T existing = await _table.FindAsync(id);
            _table.Remove(existing);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> query = _table;

            // Apply the includes if any expressions are passed
            if (includeExpressions.Any())
            {
                query = includeExpressions.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await _table.FindAsync(id);
        }

        public async Task Insert(T obj)
        {
            await _table.AddAsync(obj);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task Update(T obj)
        {
            _table.Attach(obj);
            _db.Entry(obj).State = EntityState.Modified;
        }
        public DbSet<T> Table { get { return _table; } }
    }
}
