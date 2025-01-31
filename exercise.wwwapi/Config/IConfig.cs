namespace exercise.wwwapi.Config;

public interface IConfig
{
    string? GetValue(string key);
}