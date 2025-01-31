using AutoMapper;

namespace exercise.wwwapi.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DTO.UserPost, Models.User>();
        CreateMap<DTO.BlogPostPost, Models.BlogPost>();
        
        CreateMap<Models.User, DTO.UserResponse>();
        CreateMap<Models.BlogPost, DTO.BlogPostResponse>()
            .ForMember(
                dest => dest.Author,
                opt => opt.MapFrom(src => src.Author.DisplayName)
            );
    }
}