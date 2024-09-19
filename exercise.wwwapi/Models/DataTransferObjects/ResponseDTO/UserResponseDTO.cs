namespace exercise.wwwapi.Models.DataTransferObjects.ResponseDTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public UserResponseDTO(User u)
        {
            Id = u.Id;
            UserName = u.Username;
            CreatedAt = u.CreatedAt;
            UpdatedAt = u.UpdatedAt;
        }

    }
}
