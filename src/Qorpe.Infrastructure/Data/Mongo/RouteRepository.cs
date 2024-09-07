using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Mongo;

public class RouteRepository(IMongoClient mongoClient, IHttpContextAccessor httpContextAccessor) 
    : Repository<RouteConfig>(mongoClient, httpContextAccessor), IRouteRepository<RouteConfig>
{
}
