using BrandMonitor.Application;
using BrandMonitor.Application.EntityDtoModels.CreateDtoModels;
using BrandMonitor.Application.IRepositories;
using BrandMonitor.Application.Services.Implementations;
using BrandMonitor.Application.Services.Interfaces;
using BrandMonitor.Infrastructure;
using BrandMonitor.Infrastructure.EntityModels;
using BrandMonitor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BrandMonitorTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        
        builder.Services.AddDbContext<BrandMonitorContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddAutoMapper(typeof(TaskCreateDto), typeof(TaskEntity));

        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();

        builder.Services.AddHostedService<ProcessTasksHostedService>();
        
        var app = builder.Build();
        
        app.UseHttpsRedirection();
        
        app.UseRouting();
        app.MapControllers();

        app.MigrateDatabase();
        
        app.Run();
    }
}