using AutoMapper;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DTOs.DinnerRequest;



namespace exercise.wwwapi.DTOs
{
    public class MappingProfile : Profile 
    {
        public MappingProfile() {

            //Dinner:
            CreateMap<InDinnerDTO, Dinner>();
            CreateMap<Dinner, OutDinnerDTO>();



        }
    }
}
