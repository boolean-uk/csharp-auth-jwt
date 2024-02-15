namespace exercise.wwwapi.DTO.Request
{
    public class AuthRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public bool IsValid()
        {
            return true;
        }
    }
}
