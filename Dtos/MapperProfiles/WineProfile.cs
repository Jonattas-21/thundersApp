using AutoMapper;
using Domain.Entities;
using thundersApp.Dtos;

public class WineProfile : Profile
{
    public WineProfile()
    {
        CreateMap<Wine, WineDto>().ForMember(dest => dest.Harvest, opt => opt.MapFrom(src => src.Harvest));
        CreateMap<Wine, WineDto>().ForMember(dest => dest.WineName, opt => opt.MapFrom(src => src.Name));
        CreateMap<Wine, WineDto>().ForMember(dest => dest.Winery, opt => opt.MapFrom(src => src.Winery));
        CreateMap<Wine, WineDto>().ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region));
    }
}
