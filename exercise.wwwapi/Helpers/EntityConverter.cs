namespace exercise.wwwapi.Helpers
{
    /// <summary>
    /// Helper class to convert entities to DTOs and vice versa
    /// </summary>
    public static class EntityConverter
    {
        /// <summary>
        /// Maps an entity to a another entity. For instance a model to a DTO or a modelPost to a model. Can ignore properties by specifying them in the objectsToIgnore list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inEntity">The entity that is fed</param>
        /// <param name="objectsToIgnore">A list of properties to ignore so the method does not break if there is for instance another model inside the model.</param>
        /// <returns>The in entity in another form</returns>
        public static T EntityMapper<T>(object inEntity, IEnumerable<string> objectsToIgnore = null) where T : class, new()
        {
            var outEntity = new T();

            HashSet<string> ignoreSet = null;
            if (objectsToIgnore != null)
            {
                ignoreSet = new HashSet<string>(objectsToIgnore);
            }

            foreach (var outEntityProperty in typeof(T).GetProperties())
            {
                var inEntityProperty = inEntity.GetType().GetProperty(outEntityProperty.Name);
                if (ignoreSet != null && ignoreSet.Contains(outEntityProperty.Name))
                {
                    continue;
                }
                if (inEntityProperty != null && outEntityProperty.CanWrite)
                {
                    outEntityProperty.SetValue(outEntity, inEntityProperty.GetValue(inEntity));
                }
            }
            return outEntity;
        }
    }
}
