using LiteDB;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Lite;

public class ClusterRepository(ILiteDatabase database, string tenantId) 
    : Repository<ClusterConfig>(database, tenantId), IClusterRepository<ClusterConfig>
{
}
