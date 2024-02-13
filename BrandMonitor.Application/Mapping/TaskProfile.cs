using AutoMapper;
using BrandMonitor.Application.DtoModels;
using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;

namespace BrandMonitor.Application.Mapping;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntityDto, TaskDto>()
            .ForMember(x => x.Id,
                y => y.MapFrom(src => src.Id))
            .ForMember(x => x.Guid,
                y => y.MapFrom(src => src.Guid))
            .ForMember(x => x.ChangeStateDateTimeOffset,
                y => y.MapFrom(src => src.ChangeStateDateTimeOffset))
            .ForMember(x => x.State,
                y => y.MapFrom(src => (TaskDtoState)(int)src.State))
            .ReverseMap();
    }
}