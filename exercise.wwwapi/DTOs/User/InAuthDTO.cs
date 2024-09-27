namespace exercise.wwwapi.DTOs.User
{
    public class InAuthDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public bool IsValid()
        {
            return true;
        }

    }
}
