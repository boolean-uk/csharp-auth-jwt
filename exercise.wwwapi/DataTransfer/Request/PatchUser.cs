using exercise.wwwapi.Enum;

namespace exercise.wwwapi.DataTransfer.Request
{
    public class PatchUser
    {
      public string UserName { get; set; }
        public Role Role { get; set; } // input is a number
    }
}
