using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Mongo;

public class RouteRepository : Repository<RouteConfig>, IRouteRepository<RouteConfig>
{
}
