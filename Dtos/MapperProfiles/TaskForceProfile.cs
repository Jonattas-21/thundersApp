using AutoMapper;
using Domain.Entities;

namespace thundersApp.Dtos.MapperProfiles
{
    public class TaskForceProfile : Profile
    {
        public TaskForceProfile()
        {
            CreateMap<TaskForceRequestDto, TaskForce>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<TaskForceRequestDto, TaskForce>().ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<TaskForceRequestDto, TaskForce>().ForMember(dest => dest.Assignee, opt => opt.MapFrom(src => src.Assignee));
            CreateMap<TaskForceRequestDto, TaskForce>().ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority));

            CreateMap<TaskForce, TaskForceResponseDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<TaskForce, TaskForceResponseDto>().ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<TaskForce, TaskForceResponseDto>().ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Origin.Name));
            CreateMap<TaskForce, TaskForceResponseDto>().ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority));
            CreateMap<TaskForce, TaskForceResponseDto>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
