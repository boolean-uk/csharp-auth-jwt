namespace exercise.wwwapi.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public List<Post> Posts { get; set; }
    }
}
