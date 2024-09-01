using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Mongo;

public class ClusterRepository : Repository<ClusterConfig>, IClusterRepository<ClusterConfig>
{
}
