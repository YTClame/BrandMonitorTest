using AutoMapper;
using BrandMonitor.Application.EntityDtoModels.CreateDtoModels;
using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;
using BrandMonitor.Application.EntityDtoModels.UpdateDtoModels;
using BrandMonitor.Infrastructure.EntityModels;

namespace BrandMonitor.Infrastructure.Mapping;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskEntityDto>()
            .ForMember(x => x.Id,
                y => y.MapFrom(src => src.Id))
            .ForMember(x => x.Guid,
                y => y.MapFrom(src => src.Guid))
            .ForMember(x => x.ChangeStateDateTimeOffset,
                y => y.MapFrom(src => src.ChangeStateDateTimeOffset))
            .ForMember(x => x.State,
                y => y.MapFrom(src => (TaskEntityDtoState)(int)src.State))
            .ReverseMap();
        
        CreateMap<TaskCreateDto, TaskEntity>()
            .ForMember(x => x.Guid,
                y => y.MapFrom(src => src.Guid))
            .ForMember(x => x.ChangeStateDateTimeOffset,
                y => y.MapFrom(src => src.ChangeStateDateTimeOffset))
            .ForMember(x => x.State,
                y => y.MapFrom(src => (TaskEntityState)(int)src.State))
            .ReverseMap();
        
        CreateMap<TaskUpdateDto, TaskEntity>()
            .ForMember(x => x.Id,
                y => y.MapFrom(src => src.Id))
            .ForMember(x => x.Guid,
                y => y.MapFrom(src => src.Guid))
            .ForMember(x => x.ChangeStateDateTimeOffset,
                y => y.MapFrom(src => src.ChangeStateDateTimeOffset))
            .ForMember(x => x.State,
                y => y.MapFrom(src => (TaskEntityState)(int)src.State))
            .ReverseMap();
    }
}