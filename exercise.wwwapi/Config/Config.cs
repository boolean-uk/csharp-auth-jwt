namespace exercise.wwwapi.Config;

public class Config : IConfig
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    public string? GetValue(string key)
    {
        return _configuration.GetValue<string>(key);
    }
}