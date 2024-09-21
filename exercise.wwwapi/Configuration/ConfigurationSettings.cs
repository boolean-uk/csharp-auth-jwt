namespace exercise.wwwapi.Configuration
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        IConfiguration _configuration;

        public ConfigurationSettings()
        {

            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public string getValue(string key)
        {
            return _configuration.GetValue<string>(key)!;
        }
    }
}
