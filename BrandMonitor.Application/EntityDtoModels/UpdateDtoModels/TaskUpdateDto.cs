using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;

namespace BrandMonitor.Application.EntityDtoModels.UpdateDtoModels;

public class TaskUpdateDto : BaseUpdateDto
{
    public Guid Guid { get; set; }
    public TaskEntityDtoState State { get; set; }
    public DateTimeOffset ChangeStateDateTimeOffset { get; set; }
}