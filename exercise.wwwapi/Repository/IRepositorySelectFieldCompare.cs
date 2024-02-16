using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepositorySelectFieldCompare<T> where T : class
    {
        /// <summary>
        /// Get all Objects that have the provided field and value.
        /// </summary>
        /// <param name="field"> The data field to look for a value in.</param>
        /// <param name="value"> The required value of the data field.</param>
        /// <returns>IEnumerable list of the Objects that have the provided field and the correct corresponding value.</returns>
        Task<IEnumerable<T>> GetAllWithFieldValue(string field, string value);
    }
}
