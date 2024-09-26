using exercise.Data;
using exercise.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.Infrastructure
{
    public class Repository<T>(DataContext dbContext) : IRepository<T> where T : class, IEntity
    {
        private readonly DataContext _dbContext = dbContext;

        public async Task<T> Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(string id)
        {
            T entity = await Get(id);
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Get(string id)
        {
            T entity = await _dbContext.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new ArgumentException($"No {typeof(T).Name.ToLower()} with id '{id}'");
            return entity;
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            T dbEntity = await Get(entity.Id);
            _dbContext.Set<T>().Attach(dbEntity);
            _dbContext.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
