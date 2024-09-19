
using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DatabaseContext _db;
        private DbSet<T> _table = null;

        public Repository()
        {
            _db = new DatabaseContext();
            _table = _db.Set<T>();
        }
        public Repository(DatabaseContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetById(int id)
        {
            return _table.Find(id);
        }


        public T Add(T entity)  
        {
            _table.Add(entity);
            _db.SaveChanges();
            return entity;
        }


        public T Update(T entity)
        {
            _table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
            return entity;
        }



        public void Save()
        {
            _db.SaveChanges();
        }

     
    }  
}
