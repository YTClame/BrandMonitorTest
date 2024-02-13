namespace BrandMonitor.Application.EntityDtoModels.EntityDtoModels;

public class TaskEntityDto : BaseEntityDto
{
    public Guid Guid { get; set; }
    public TaskEntityDtoState State { get; set; }
    public DateTimeOffset ChangeStateDateTimeOffset { get; set; }
}

public enum TaskEntityDtoState
{
    Default = 0,
    Created = 1,
    Running = 2,
    Finished = 3,
}