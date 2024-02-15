namespace exercise.wwwapi.Models.InputModels
{
    /// <summary>
    /// Data transfer object that contains information regarding a request to authenticate a user account.
    /// </summary>
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
