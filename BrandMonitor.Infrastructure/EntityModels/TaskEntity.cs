namespace BrandMonitor.Infrastructure.EntityModels;

public class TaskEntity : BaseEntity
{
    public Guid Guid { get; set; }
    public TaskEntityState State { get; set; }
    public DateTimeOffset ChangeStateDateTimeOffset { get; set; }
}

public enum TaskEntityState
{
    Default = 0,
    Created = 1,
    Running = 2,
    Finished = 3,
}