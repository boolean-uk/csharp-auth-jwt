namespace exercise.wwwapi.Models.InputModels
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
