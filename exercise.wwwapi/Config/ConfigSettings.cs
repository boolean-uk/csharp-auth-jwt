namespace exercise.wwwapi.Config
{
    public class ConfigSettings : IConfigSettings
    {
        IConfiguration _configuration;
        public ConfigSettings()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        public string GetValue(string key)
        {
            return _configuration.GetValue<string>(key)!;
        }
    }
}
