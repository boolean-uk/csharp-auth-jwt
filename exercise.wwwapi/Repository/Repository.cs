﻿using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        DataContext _db;
        private DbSet<T> _table_T = null;

        public Repository(DataContext db)
        {
            _db = db;
            _table_T = _db.Set<T>();
        }

        /// <inheritdoc/>
        public async Task<T?> Get(int id)
        {
            return await _table_T.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table_T.ToListAsync();
        }


        /// <inheritdoc/>
        public async Task<T> Insert(T entity)
        {
            var addedEntity = _table_T.Add(entity);
            await _db.SaveChangesAsync();
            return addedEntity.Entity;
        }

        /// <inheritdoc/>
        public async Task<T> Update(int id, T entity)
        {
            var dbEntity = await _table_T.FindAsync(id);
            // Update values of the dbEntity
            _db.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync(); // Save changes
            return dbEntity;
        }

        /// <inheritdoc/>
        public async Task<T> Delete(int id)
        {
            T? entityToDelete = await _table_T.FindAsync(id);
            _table_T.Remove(entityToDelete);
            await _db.SaveChangesAsync();
            return entityToDelete;
        }

        /// <inheritdoc/>
        public async Task<T> Delete(T entity)
        {
            _table_T.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        /// <remarks>Made with help from GPT3.5</remarks>
        public async Task<IEnumerable<T>> GetAllWithFieldValue(string field, string value) 
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
            MemberExpression property = Expression.Property(parameter, field);
            var constant = Expression.Constant(Convert.ChangeType(value, property.Type));
            var equalExpression = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equalExpression, parameter);

            return await _table_T.Where(lambda).ToListAsync();
        }
    }
}
