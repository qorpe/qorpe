using LiteDB;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Lite;

public class RouteRepository(ILiteDatabase database, string tenantId) 
    : Repository<RouteConfig>(database, tenantId), IRouteRepository<RouteConfig>
{
}
