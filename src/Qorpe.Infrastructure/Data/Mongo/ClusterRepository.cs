using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Mongo;

public class ClusterRepository(IMongoDatabase database) : Repository<ClusterConfig>(database), IClusterRepository
{
}
