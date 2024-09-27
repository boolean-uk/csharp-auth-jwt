using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DataModels.DTOs
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; } // Normally should not be included in DTOs, but I'm including it here for fun
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
