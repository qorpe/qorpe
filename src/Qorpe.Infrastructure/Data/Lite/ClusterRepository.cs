using LiteDB;
using Microsoft.AspNetCore.Http;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Lite;

public class ClusterRepository(ILiteDatabase database, IHttpContextAccessor httpContextAccessor) 
    : Repository<ClusterConfig>(database, httpContextAccessor), IClusterRepository
{
}
