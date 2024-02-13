using BrandMonitor.Application.DtoModels;
using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;

namespace BrandMonitor.Application.Services.Interfaces;

public interface ITaskService
{
    Task<TaskDto> CreateAndRunBusinessTask();

    Task<(bool isExist, TaskDtoState taskState)> GetStatusOfTaskForGuid(Guid taskGuid);

    Task RunTaskExecutionProcess(TaskEntityDto task, int? processTimeInSeconds = null);

    int GetTaskProcessTimeInSeconds();
}