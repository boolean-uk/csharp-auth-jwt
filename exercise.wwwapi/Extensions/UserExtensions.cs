using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Extensions
{
    public static class UserExtensions
    {

        public static UserDTO ToDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                PasswordHash = user.PasswordHash
            };
        }
    }
}
