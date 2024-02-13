using System.Text.Json;
using System.Text.Json.Serialization;
using BrandMonitor.Application.Services.Interfaces;
using BrandMonitorTest.ResponseDto;
using Microsoft.AspNetCore.Mvc;

namespace BrandMonitorTest.Controllers;

public class TaskController : Controller
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    [HttpPost("/task")]
    public async Task<IActionResult> HandleCreateAndRunTaskRequest()
    {
        var taskDto = await _taskService.CreateAndRunBusinessTask();
        
        HttpContext.Response.StatusCode = StatusCodes.Status202Accepted;
        var responseData = new CreateAndRunTaskResponseDto()
        {
            TaskGuid = taskDto.Guid,
        };
        
        return Json(responseData);
    }

    [HttpGet("/task/{taskGuid}")]
    public async Task<IActionResult> HandleGetTaskStateRequest(Guid taskGuid)
    {
        if (taskGuid == Guid.Empty)
            return BadRequest();

        var dataAboutTask = await _taskService.GetStatusOfTaskForGuid(taskGuid);

        if (!dataAboutTask.isExist)
            return NotFound();

        var responseData = new TaskStateResponseDto()
        {
            TaskState = dataAboutTask.taskState,
        };

        return Json(
            responseData,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
    }
}