using exercise.wwwapi.Data;
using exercise.wwwapi.Model;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DataContext _db { get; set; }
        public Repository(DataContext db) 
        {
            this._db = db;
        }

        public async Task<List<Codes>> GetCodes() 
        {
            return await this._db.codes.ToListAsync();
        }
    }
}
