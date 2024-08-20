using AutoMapper;
using Domain.Entities;

namespace thundersApp.Dtos.MapperProfiles
{
    public class AnalysisProfile : Profile
    {
        public AnalysisProfile()
        {
            CreateMap<AnalysisRequestDto, Analysis>().ForMember(dest => dest.Alcohol, opt => opt.MapFrom(src => src.Alcohol));
            CreateMap<AnalysisRequestDto, Analysis>().ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body));
            CreateMap<AnalysisRequestDto, Analysis>().ForMember(dest => dest.Tannin, opt => opt.MapFrom(src => src.Tannin));
            CreateMap<AnalysisRequestDto, Analysis>().ForMember(dest => dest.Acidity, opt => opt.MapFrom(src => src.Acidity));
            CreateMap<AnalysisRequestDto, Analysis>().ForMember(dest => dest.AdditionNotes, opt => opt.MapFrom(src => src.AdditionNotes));
            CreateMap<AnalysisRequestDto, Analysis>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WineId));

            CreateMap<Analysis, AnalysisResponseDto>().ForMember(dest => dest.Alcohol, opt => opt.MapFrom(src => src.Alcohol));
            CreateMap<Analysis, AnalysisResponseDto>().ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body));
            CreateMap<Analysis, AnalysisResponseDto>().ForMember(dest => dest.Tannin, opt => opt.MapFrom(src => src.Tannin));
            CreateMap<Analysis, AnalysisResponseDto>().ForMember(dest => dest.Acidity, opt => opt.MapFrom(src => src.Acidity));
            CreateMap<Analysis, AnalysisResponseDto>().ForMember(dest => dest.AdditionNotes, opt => opt.MapFrom(src => src.AdditionNotes));
        }
    }
}
