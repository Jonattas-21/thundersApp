using AutoMapper;
using Domain.Entities;

namespace thundersApp.Dtos.MapperProfiles
{
    public class GrapeProfile : Profile
    {
        public GrapeProfile()
        {
            CreateMap<WineRequestDto, Grape>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GrapeName));
            CreateMap<WineRequestDto, Grape>().ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.GrapeDescription));
            CreateMap<WineRequestDto, Grape>().ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.GrapeOrigin));

            CreateMap<Grape, GrapeResponseDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<Grape, GrapeResponseDto>().ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<Grape, GrapeResponseDto>().ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Origin));
        }
    }
}
