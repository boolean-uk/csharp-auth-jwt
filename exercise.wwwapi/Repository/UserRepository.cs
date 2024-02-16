using exercise.wwwapi.Data;
using exercise.wwwapi.Models.PureModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class UserRepository : IRepositorySelectFieldCompare<ApplicationUser>
    {
        DataContext _db;
        private DbSet<ApplicationUser>? _table = null;

        public UserRepository(DataContext db)
        {
            _db = db;
            _table = _db.Set<ApplicationUser>();
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> Get(string id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<bool> UserIsAdmin(string id) 
        {
            ApplicationUser user = await _table.FindAsync(id);

            return user.Role == Enums.Role.Administrator;
        }

        /// <inheritdoc/>
        /// <remarks>Made with help from GPT3.5</remarks>
        public async Task<IEnumerable<ApplicationUser>> GetAllWithFieldValue(string field, string value)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(ApplicationUser), "e");
            MemberExpression property = Expression.Property(parameter, field);
            var constant = Expression.Constant(Convert.ChangeType(value, property.Type));
            var equalExpression = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<ApplicationUser, bool>>(equalExpression, parameter);

            return await _table.Where(lambda).ToListAsync();
        }
    }
}
