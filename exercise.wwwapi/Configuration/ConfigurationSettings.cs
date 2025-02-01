namespace exercise.wwwapi.Configuration
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        IConfiguration _conf; 
        public ConfigurationSettings() 
        {
            _conf = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        public T GetValue<T>(string key)
        {
            return _conf.GetValue<T>(key)!;
        }
    }
}
