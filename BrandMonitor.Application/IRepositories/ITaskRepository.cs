using BrandMonitor.Application.EntityDtoModels.CreateDtoModels;
using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;
using BrandMonitor.Application.EntityDtoModels.UpdateDtoModels;

namespace BrandMonitor.Application.IRepositories;

public interface ITaskRepository : IRepository<TaskEntityDto, TaskCreateDto, TaskUpdateDto>
{
    Task UpdateTaskStateForGuid(
        Guid taskGuid,
        TaskEntityDtoState newTaskState);

    Task<TaskEntityDto?> GetTaskByGuid(
        Guid taskGuid);

    Task<IEnumerable<TaskEntityDto>> GetTasksWithState(
        TaskEntityDtoState state);
}