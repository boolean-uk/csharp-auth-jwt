using AutoMapper;
using exercise.wwwapi.Models;
using exercise.wwwapi.DTOs;

namespace exercise.wwwapi.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<User, UserResponseDTO>();

            CreateMap<Blog, BlogResponseDTO>();
            CreateMap<BlogPOST, Blog>();
            CreateMap<BlogPUT, Blog>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null && (!srcMember.GetType().IsValueType || !Equals(srcMember, 0))));
        }
    }
}
