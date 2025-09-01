using Microsoft.EntityFrameworkCore;
using Qorpe.Hub.Domain.Entities;

namespace Qorpe.Hub.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Tenant> Tenants { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}