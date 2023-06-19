using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Services.DTO;

namespace BBMS.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bar, BarDTO>();
            CreateMap<Bar, BarDTO>().ReverseMap();

            CreateMap<Beer, BeerDTO>();
            CreateMap<Beer, BeerDTO>().ReverseMap();

            CreateMap<Brewery, BreweryDTO>();
            CreateMap<Brewery, BreweryDTO>().ReverseMap();

        }
    }
}
