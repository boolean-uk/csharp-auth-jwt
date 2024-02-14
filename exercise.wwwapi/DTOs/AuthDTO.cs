using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DTOs
{
    public record RegisterDto(string Email, string Password);// not passing a role here, as they will automatically be assigned a user, let an admin upgrade
    // the user to admin
    public record RegisterResponseDto(string Email, UserRole Role);// return back Email and user role
    public record LoginDto(string Email, string Password); // pass in the email and password, when logging in
    public record AuthResponseDto(string Token, string Email, UserRole Role);//when authenticating and logging in, want to send back the generated token, 
    // the user email and let the application now which role the user has
}
