using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepositorySelectFieldCompare<T> where T : class
    {
        Task<IEnumerable<T>> GetAllWithFieldValue(string field, string value);
    }
}
