using LiteDB;
using Microsoft.AspNetCore.Http;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Lite;

public class RouteRepository(ILiteDatabase database, IHttpContextAccessor httpContextAccessor) 
    : Repository<RouteConfig>(database, httpContextAccessor), IRouteRepository
{
}
