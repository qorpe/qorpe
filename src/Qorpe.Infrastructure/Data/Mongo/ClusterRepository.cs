using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Mongo;

public class ClusterRepository(IMongoClient mongoClient, IHttpContextAccessor httpContextAccessor) 
    : Repository<ClusterConfig>(mongoClient, httpContextAccessor), IClusterRepository
{
}
