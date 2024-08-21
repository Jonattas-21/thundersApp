using AutoMapper;
using Domain.Entities;

namespace thundersApp.Dtos.MapperProfiles
{
    public class OriginProfile : Profile
    {
        public OriginProfile()
        {
            CreateMap<Origin, OriginResponseDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<Origin, OriginResponseDto>().ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
