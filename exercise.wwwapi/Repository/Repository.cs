using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<Model> : IRepository<Model> where Model : class
    {
        private DatabaseContext _db;
        private DbSet<Model> _table;

        public Repository(DatabaseContext db)
        {
            _db = db;
            _table = _db.Set<Model>();
        }

        public async Task<Model> Create(Model model)
        {
            _table.Add(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<Model>> GetAll(Expression<Func<Model, bool>> predicate)
        {
            return await _table.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Model>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<Model> Get(Expression<Func<Model, bool>> predicate)
        {
            return await _table.FirstOrDefaultAsync(predicate);
        }

        public async Task<Model> Update(Model model)
        {
            _table.Attach(model);
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<Model> Delete(Model model)
        {
            _table.Attach(model);
            await _db.Entry(model).ReloadAsync();

            _table.Remove(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public async void Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}