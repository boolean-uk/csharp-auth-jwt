using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {


        private DataContext _db;
        private DbSet<T> _table = null;

        public Repository(DataContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }


        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }
        public async Task<T> GetById(object id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T> Insert(T obj)
        {
            var result = await _table.AddAsync(obj);
             await _db.SaveChangesAsync();
            return result.Entity;
        }
        public async Task Update(T obj)
        {
            _db.Set<T>().Update(obj);
            await _db.SaveChangesAsync();
        }




        public async Task<T> Delete(object id)
        {
            T existing = await _table.FindAsync(id);
             _table.Remove(existing);
            await _db.SaveChangesAsync();
            return existing;
        }


        public DbSet<T> Table { get { return _table; } }

    }
}

