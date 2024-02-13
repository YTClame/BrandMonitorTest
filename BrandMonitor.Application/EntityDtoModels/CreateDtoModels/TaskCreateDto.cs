using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;

namespace BrandMonitor.Application.EntityDtoModels.CreateDtoModels;

public class TaskCreateDto : BaseCreateDto
{
    public Guid Guid { get; set; }
    public TaskEntityDtoState State { get; set; }
    public DateTimeOffset ChangeStateDateTimeOffset { get; set; }
}