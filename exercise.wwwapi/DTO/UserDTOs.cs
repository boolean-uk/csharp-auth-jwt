namespace exercise.wwwapi.DTO
{
    public record UserRegisterPost(string Username, string Password, string Email);
    public record UserLoginPost(string Password, string Email);
    public record UserSimple(string UserName);
}
