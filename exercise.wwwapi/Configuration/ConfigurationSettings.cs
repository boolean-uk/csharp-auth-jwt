using System.Configuration;

namespace exercise.wwwapi.Configuration
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        IConfiguration _configuration;
        public ConfigurationSettings()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        public string GetValue(string key)
        {
            return _configuration.GetValue<string>(key)!;
        }
    }
}