using exercise.wwwapi.DataModels;

namespace exercise.wwwapi.Services
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUser user);
    }
}
