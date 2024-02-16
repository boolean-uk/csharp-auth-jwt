namespace exercise.wwwapi.DataTransfer.Request
{
    public class AuthRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool isValid()
        {
            return true;
        }
    }
}
