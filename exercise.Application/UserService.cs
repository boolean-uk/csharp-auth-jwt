using exercise.Data.Models;
using exercise.Infrastructure;

namespace exercise.Application
{
    public class UserService
    {
        private readonly IRepository<User> _repository;
        public UserService(IRepository<User> repository) 
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<User>> Get(string id)
        {
            ServiceResponse<User> response = new();
            try
            {
                User user = await _repository.Get(id);
                response.Data = user;
            } catch (ArgumentException ex) 
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
