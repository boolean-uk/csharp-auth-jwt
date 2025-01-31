namespace exercise.wwwapi.DTO;

public class UserResponse
{
    public Guid Id { get; set; }
    public required string DisplayName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
}