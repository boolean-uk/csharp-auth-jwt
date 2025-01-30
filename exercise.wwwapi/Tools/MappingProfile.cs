using AutoMapper;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Tools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CommentRequestDto, Comment>();
            CreateMap<CommentResponseDto, Comment>();
            CreateMap<BlogPostResponseDto, BlogPost>();
            CreateMap<BlogPostWithCommentsDto, BlogPost>();
            CreateMap<BlogPost, BlogPostWithCommentsDto>();
            CreateMap<User, UserResponseDto>();

            CreateMap<BlogPostRequestDto, BlogPost>();
            CreateMap<BlogPost, BlogPostResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<Comment, CommentResponseDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
        }
    }
}
