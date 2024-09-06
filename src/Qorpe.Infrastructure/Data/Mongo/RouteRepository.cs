using MongoDB.Driver;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Mongo;

public class RouteRepository(IMongoDatabase database) 
    : Repository<RouteConfig>(database), IRouteRepository<RouteConfig>
{
}
