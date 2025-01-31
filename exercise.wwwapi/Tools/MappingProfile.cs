using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Models;

namespace api_cinema_challenge.Tools
{
    public class MappingProfile : Profile
    {

        public MappingProfile() 
        { 
            CreateMap<User, UserDTO>();
        }
    }
}
