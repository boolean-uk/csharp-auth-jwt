namespace exercise.wwwapi.DTOs
{
    public class UserWithBlogsResponseDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public ICollection<BlogResponseDTO> Blogs { get; set; }
    }
}
