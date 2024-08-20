using AutoMapper;
using Domain.Entities;
using thundersApp.Dtos;

public class WineProfile : Profile
{
    public WineProfile()
    {
        CreateMap<WineRequestDto, Wine>().ForMember(dest => dest.Harvest, opt => opt.MapFrom(src => src.Harvest));
        CreateMap<WineRequestDto, Wine>().ForMember(dest => dest.Winery, opt => opt.MapFrom(src => src.Winery));
        CreateMap<WineRequestDto, Wine>().ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region));
        CreateMap<WineRequestDto, Wine>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WineName));

        CreateMap<Wine, WineResponseDto>().ForMember(dest => dest.Harvest, opt => opt.MapFrom(src => src.Harvest));
        CreateMap<Wine, WineResponseDto>().ForMember(dest => dest.Winery, opt => opt.MapFrom(src => src.Winery));
        CreateMap<Wine, WineResponseDto>().ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region));
        CreateMap<Wine, WineResponseDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<Wine, WineResponseDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}
