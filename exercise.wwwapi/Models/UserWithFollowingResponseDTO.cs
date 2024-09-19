namespace exercise.wwwapi.Models
{
    public class UserWithFollowingResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public List<FollowerDTO> Following { get; set; } = new List<FollowerDTO>();
    }
}
