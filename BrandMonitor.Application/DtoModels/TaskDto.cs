namespace BrandMonitor.Application.DtoModels;

public class TaskDto : BaseDto
{
    public Guid Guid { get; set; }
    public TaskDtoState State { get; set; }
    public DateTimeOffset ChangeStateDateTimeOffset { get; set; }
}

public enum TaskDtoState
{
    Default = 0,
    Created = 1,
    Running = 2,
    Finished = 3,
}