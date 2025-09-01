using Microsoft.EntityFrameworkCore;
using Qorpe.Scheduler.Application.Common.Interfaces;

namespace Qorpe.Scheduler.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IAppDbContext
{
    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        
        b.HasDefaultSchema("scheduler");
        
        b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}