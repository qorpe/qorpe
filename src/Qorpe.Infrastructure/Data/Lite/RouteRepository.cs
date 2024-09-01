using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Lite;

public class RouteRepository : Repository<RouteConfig>, IRouteRepository<RouteConfig>
{
}
