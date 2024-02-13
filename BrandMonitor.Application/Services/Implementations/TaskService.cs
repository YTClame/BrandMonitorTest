using AutoMapper;
using BrandMonitor.Application.DtoModels;
using BrandMonitor.Application.EntityDtoModels.CreateDtoModels;
using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;
using BrandMonitor.Application.IRepositories;
using BrandMonitor.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BrandMonitor.Application.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public TaskService(
        ITaskRepository taskRepository,
        IMapper mapper,
        IConfiguration configuration)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _configuration = configuration;
    }
    
    public async Task<TaskDto> CreateAndRunBusinessTask()
    {
        var taskEntityDto = await CreateTask();
        
        Task.Run(async () =>
        {
            await RunTaskExecutionProcess(taskEntityDto);
        });
        
        var taskDto = _mapper.Map<TaskEntityDto, TaskDto>(taskEntityDto);
        return taskDto;
    }

    public async Task<(bool isExist, TaskDtoState taskState)> GetStatusOfTaskForGuid(Guid taskGuid)
    {
        var task = await _taskRepository.GetTaskByGuid(taskGuid);

        return task == null
            ? (isExist: false, taskState:  TaskDtoState.Default)
            : (isExist: true, taskState: (TaskDtoState)(int)task.State);
    }

    private async Task<TaskEntityDto> CreateTask()
    {
        return await _taskRepository.Create(new TaskCreateDto()
        {
            ChangeStateDateTimeOffset = DateTimeOffset.UtcNow,
            Guid = Guid.NewGuid(),
            State = (TaskEntityDtoState)(int)TaskDtoState.Created,
        });
    }

    public async Task RunTaskExecutionProcess(TaskEntityDto task, int? processTimeInSeconds = null)
    {
        await RunTask(task);

        if (processTimeInSeconds == null)
            processTimeInSeconds = GetTaskProcessTimeInSeconds();
            
        await Task.Delay(new TimeSpan(0, 0, processTimeInSeconds.Value));

        await FinishTask(task);
    }

    public int GetTaskProcessTimeInSeconds()
    {
        var processTimeEnvName = "TaskProgressTimeInSeconds";
        var processTime = _configuration.GetSection(processTimeEnvName).Value;
        if (string.IsNullOrEmpty(processTime))
            throw new Exception($"Environment variable \"{processTimeEnvName}\" can't be a null or empty!");
        return Convert.ToInt32(processTime);
    }

    private async Task RunTask(TaskEntityDto task)
    {
        await _taskRepository.UpdateTaskStateForGuid(
            task.Guid,
            TaskEntityDtoState.Running);
    }

    private async Task FinishTask(TaskEntityDto task)
    {
        await _taskRepository.UpdateTaskStateForGuid(
            task.Guid,
            TaskEntityDtoState.Finished);
    }
}