using BrandMonitor.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace BrandMonitor.Infrastructure;

public class BrandMonitorContext : DbContext
{
    public BrandMonitorContext(DbContextOptions<BrandMonitorContext> optionsBuilder)
        : base(optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskEntity>().HasIndex(x => x.Guid).IsUnique();
    }

    public DbSet<TaskEntity> Tasks { get; set; }
}