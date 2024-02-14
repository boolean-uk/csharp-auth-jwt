namespace exercise.wwwapi.Repository
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieve all entires from the database
        /// </summary>
        /// <returns>IEnumerable of entities</returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Get a specific entry based on its database ID
        /// </summary>
        /// <param name="id">int - The ID of the database entry</param>
        /// <returns>Entity from the database matching the provided ID, null if not matching entries</returns>
        Task<T?> Get(int id);

        /// <summary>
        /// Insert a provided entity into the database
        /// </summary>
        /// <param name="entity">The entity to be attached to the database</param>
        /// <returns>Entity that was attached to the database</returns>
        Task<T> Insert(T entity);

        /// <summary>
        /// Update one or several fields of a entity from the database.
        /// </summary>
        /// <param name="id">The original entity's database ID</param>
        /// <param name="entity">The new values for the entity object</param>
        /// <returns>The updated entity object</returns>
        Task<T> Update(int id, T entity);

        /// <summary>
        /// Delete a entity from the database that has a corresponding ID.
        /// </summary>
        /// <param name="id">int - the ID of the database entry to remove</param>
        /// <returns>The entity removed</returns>
        Task<T> Delete(int id);

        /// <summary>
        /// Delete a entity from the database.
        /// </summary>
        /// <param name="entity">T - the entity object to remove from the database. Has to exist as provided in the database to be removed.</param>
        /// <returns>The entity removed</returns>
        Task<T> Delete(T entity);

    }
}
