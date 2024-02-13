using AutoMapper;
using BrandMonitor.Application.EntityDtoModels.CreateDtoModels;
using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;
using BrandMonitor.Application.EntityDtoModels.UpdateDtoModels;
using BrandMonitor.Application.IRepositories;
using BrandMonitor.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BrandMonitor.Infrastructure.Repositories;

public class TaskRepository : BaseRepository<TaskEntity, TaskEntityDto, TaskCreateDto, TaskUpdateDto>, ITaskRepository
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public TaskRepository(
        BrandMonitorContext context,
        IMapper mapper,
        IServiceScopeFactory serviceScopeFactory) : base(context, mapper)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task UpdateTaskStateForGuid(
        Guid taskGuid,
        TaskEntityDtoState newTaskState)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BrandMonitorContext>();
        var entity = await context.Tasks
            .FirstOrDefaultAsync(w => w.Guid == taskGuid);

        if (entity == null)
            throw new Exception($"Can't find {typeof(TaskEntity)} with {nameof(entity.Guid)} == {taskGuid}");

        entity.State = (TaskEntityState)(int)newTaskState;
        entity.ChangeStateDateTimeOffset = DateTimeOffset.UtcNow;
        
        await context.SaveChangesAsync();
        
        context.ChangeTracker.Clear();
            
        await context.DisposeAsync();
    }

    public async Task<TaskEntityDto?> GetTaskByGuid(Guid taskGuid)
    {
        var taskEntity = await Context.Tasks
            .FirstOrDefaultAsync(w => w.Guid == taskGuid);
        
        return taskEntity == null
            ? null
            : Mapper.Map<TaskEntity, TaskEntityDto>(taskEntity);
    }

    public async Task<IEnumerable<TaskEntityDto>> GetTasksWithState(TaskEntityDtoState state)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BrandMonitorContext>();

        var tasks = await context.Tasks
            .Where(w => w.State == (TaskEntityState)(int)state)
            .AsNoTracking()
            .ToListAsync();
        
        return Mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskEntityDto>>(tasks);
    }
}