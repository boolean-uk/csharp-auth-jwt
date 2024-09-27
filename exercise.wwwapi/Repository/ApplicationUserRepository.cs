using exercise.wwwapi.Data;
using exercise.wwwapi.DataModels.Models;
using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class ApplicationUserRepository
    {
        private readonly DataContext _db;
        private readonly DbSet<ApplicationUser> _table;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public ApplicationUserRepository(DataContext dataContext, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _db = dataContext;
            _table = _db.Set<ApplicationUser>();
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<ApplicationUser>> SelectAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<ApplicationUser> SelectById(object id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<ApplicationUser> Delete(ApplicationUser entity)
        {
            _db.Entry(entity).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<ApplicationUser> Insert(ApplicationUser entity)
        {
            _table.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Result<ApplicationUser>> InsertUser(ApplicationUser user, string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(user, password);
            user.PasswordHash = hashedPassword;

            _table.Add(user);
            await _db.SaveChangesAsync();
            return Result<ApplicationUser>.Success(user);
        }

        public async Task<ApplicationUser> Update(ApplicationUser entity)
        {
            _table.Update(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<ApplicationUser> SelectByEmail(string email)
        {
            return await _table.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
