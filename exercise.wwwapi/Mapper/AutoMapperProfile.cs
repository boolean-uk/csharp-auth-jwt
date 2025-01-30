using AutoMapper;

namespace exercise.wwwapi.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DTO.UserPost, Models.User>();
        
        CreateMap<Models.User, DTO.UserResponse>();
    }
}