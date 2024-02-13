using BrandMonitor.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BrandMonitorTest;

public static class MigrationManager
{
    public static void MigrateDatabase(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<BrandMonitorContext>();
        appContext.Database.Migrate();
    }
}