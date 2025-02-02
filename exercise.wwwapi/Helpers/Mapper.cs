using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Helpers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDTO>(); 
            CreateMap<Blog, BlogDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            //CreateMap<Blog, BlogDTO>();
                //.ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<Blog, AddBlogDTO>();
            CreateMap<UpdateDTO, Blog>(); 
        }


    }
}
