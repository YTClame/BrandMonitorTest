using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;
using BrandMonitor.Application.IRepositories;
using BrandMonitor.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrandMonitor.Application;

/// <summary>
/// Сервис для расчёта оставшегося времени выполнения незавершенных задач, их "запуска".
/// </summary>
public class ProcessTasksHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
 
    public ProcessTasksHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
 
    public Task StartAsync(CancellationToken cancellationToken)
    {
        DoSomeWorkEveryFiveSecondsAsync(cancellationToken);
        return Task.CompletedTask;
    }
 
    private async Task DoSomeWorkEveryFiveSecondsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
        
        var tasks = await taskRepository.GetTasksWithState(TaskEntityDtoState.Running);
        var now = DateTimeOffset.UtcNow;
        var processTimeInSeconds = taskService.GetTaskProcessTimeInSeconds();
        
        foreach (var task in tasks)
        {
            var timeLeft = now - task.ChangeStateDateTimeOffset;
            if (timeLeft >= new TimeSpan(0, 0, processTimeInSeconds))
                taskService.RunTaskExecutionProcess(task, 0);
            else
                taskService.RunTaskExecutionProcess(task, Convert.ToInt32(Math.Round(processTimeInSeconds - timeLeft.TotalSeconds)));
        }
        
        tasks = await taskRepository.GetTasksWithState(TaskEntityDtoState.Created);
        foreach (var task in tasks)
            taskService.RunTaskExecutionProcess(task);
    }
 
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}