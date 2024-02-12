
using auth.Model;

namespace auth.DTO {
    public record AuthResponseDto(string Token, string Email, UserRole Role, string Id);
}