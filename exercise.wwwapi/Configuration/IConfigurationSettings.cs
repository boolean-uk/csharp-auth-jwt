namespace exercise.wwwapi.Configuration
{
    public interface IConfigurationSettings
    {
        public T GetValue<T>(string key);
    }
}
