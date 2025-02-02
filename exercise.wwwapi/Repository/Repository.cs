using System;
using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository;

public class Repository<T> : IRepository<T> where T : class, IBlogEntity
{
    private DataContext _db;
    private DbSet<T> _table;

    public Repository(DataContext context)
    {
        _db = context;
        _table = _db.Set<T>();
    }

   
    public async Task<T> CreateEntity(T entity)
    {
        
        if (_table.Count() == 0)
        {
            entity.Id = 1;
        }
        else
        {
            entity.Id = _table.Max(e => e.Id) + 1;
        }
        entity.CreatedAt = DateTime.Now.ToUniversalTime();
        entity.UpdatedAt = DateTime.Now.ToUniversalTime();
        await _table.AddAsync(entity);
        await _db.SaveChangesAsync(); 
        return entity;
       
    }

    public async Task<T> DeleteEntityById(int id)
    {
        T entity = await _table.FindAsync(id);
        if (entity != null)
        {
            _table.Remove(entity);
            await _db.SaveChangesAsync();
        }
        
        return entity;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _table.ToListAsync();
    }

    //The use of "lazy loading" enables us to get all relationed entities without include
    public async Task<T> GetEntityById(int id)
    {
        return await _table.FindAsync(id);
    }

    public async Task<T> UpdateEntityById(int id, T entity)
    {
        T entityToUpdate = await _table.FindAsync(id);

        try
        {
            entityToUpdate.Update(entity);
            entityToUpdate.UpdatedAt = DateTime.Now.ToUniversalTime();
            await _db.SaveChangesAsync();

            return entityToUpdate;
        }
        catch (Exception)
        {
            return entityToUpdate;
        }
    }

    public bool Exists(Func<T, bool> exist)
    {
        return _table.Any(exist);
    }
}
