using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Models;
using System.Numerics;

namespace exercise.wwwapi.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserFollower, UserFollowerDTO>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<BlogPost, BlogPostDTO>();
            CreateMap<Comment, CommentDTO>();
        }
    }
}
