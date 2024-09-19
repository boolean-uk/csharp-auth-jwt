﻿using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<Model> : IRepository<Model>
        where Model : class
    {
        private DataContext _db;
        private DbSet<Model> _dbSet;

        public Repository(DataContext db)
        {
            _db = db;
            _dbSet = _db.Set<Model>();
        }

        public async Task<Model> Create(string[] inclusions, Model model)
        {
            _dbSet.Add(model);
            await _db.SaveChangesAsync();
            foreach (string inclusion in inclusions)
            {
                await _db.Entry(model).Reference(inclusion).LoadAsync();
            }
            return model;
        }

        public async Task<IEnumerable<Model>> GetAll(string[] inclusions, Expression<Func<Model, bool>> predicate)
        {
            var query = _dbSet.AsQueryable();
            foreach (string inclusion in inclusions)
            {
                query = query.Include(inclusion);
            }
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Model>> GetAll(string[] inclusions)
        {
            var query = _dbSet.AsQueryable();
            foreach (string inclusion in inclusions)
            {
                query = query.Include(inclusion);
            }
            return await query.ToListAsync();
        }

        public async Task<Model> Get(string[] inclusions, Expression<Func<Model, bool>> predicate)
        {
            var query = _dbSet.AsQueryable();
            foreach (string inclusion in inclusions)
            {
                query = query.Include(inclusion);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<Model> Update(string[] inclusions, Model model)
        {
            _dbSet.Attach(model);
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            foreach (string inclusion in inclusions)
            {
                await _db.Entry(model).Reference(inclusion).LoadAsync();
            }
            return model;
        }

        public async Task<Model> Delete(string[] inclusions, Model model)
        {
            // get original values from the database...
            _dbSet.Attach(model);
            await _db.Entry(model).ReloadAsync();

            foreach (string inclusion in inclusions)
            {
                await _db.Entry(model).Reference(inclusion).LoadAsync();
            }

            _dbSet.Remove(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public Task<IEnumerable<Model>> GetAll()
        {
            return GetAll([]);
        }

        public Task<IEnumerable<Model>> GetAll(Expression<Func<Model, bool>> predicate)
        {
            return GetAll([], predicate);
        }

        public Task<Model> Get(Expression<Func<Model, bool>> predicate)
        {
            return Get([], predicate);
        }

        public Task<Model> Create(Model model)
        {
            return Create([], model);
        }

        public Task<Model> Update(Model model)
        {
            return Update([], model);
        }

        public Task<Model> Delete(Model model)
        {
            return Delete([], model);
        }
    }
}