using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Application.Common.Models;
using Qorpe.Domain.Entities;
using System.Linq.Expressions;

namespace Qorpe.Application.Features.Routes.Queries.GetRoutes;

public class GetRoutesQueryHandler(IRouteRepository<RouteConfig> routeRepository) : IRequestHandler<GetRoutesQuery, PaginatedResponse<RouteConfigDto>>
{
    public async Task<PaginatedResponse<RouteConfigDto>> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
    {
        if (request.QueryParameters.Page < 1 || request.QueryParameters.PageSize <= 0)
        {
            throw new Exception("Invalid pagination parameters.");
        }

        var filterExpression = BuildFilterExpression(id, tenantId, createdAt, updatedAt, routeId);
        var sortExpression = BuildSortExpression(sort);

        var routes = routeRepository.FilterBy(filterExpression)
                                          .OrderBy(sortExpression)
                                          .Skip((5 - 1) * 5)
                                          .Take(5)
                                          .ToList();

        throw new NotImplementedException();
    }

    private Expression<Func<RouteConfig, bool>> BuildFilterExpression(
        string? id,
        string? tenantId,
        DateTime? createdAt,
        DateTime? updatedAt,
        string? routeId)
    {
        var parameter = Expression.Parameter(typeof(RouteConfig), "x");
        Expression combinedExpression = null;

        if (!string.IsNullOrEmpty(id))
        {
            var property = Expression.Property(parameter, nameof(RouteConfig.Id));
            var constant = Expression.Constant(id);
            var comparison = Expression.Equal(property, constant);
            combinedExpression = combinedExpression == null
                ? comparison
                : Expression.AndAlso(combinedExpression, comparison);
        }

        if (!string.IsNullOrEmpty(tenantId))
        {
            var property = Expression.Property(parameter, nameof(RouteConfig.TenantId));
            var constant = Expression.Constant(tenantId);
            var comparison = Expression.Equal(property, constant);
            combinedExpression = combinedExpression == null
                ? comparison
                : Expression.AndAlso(combinedExpression, comparison);
        }

        // Additional filters can be added similarly

        return combinedExpression != null ? Expression.Lambda<Func<RouteConfig, bool>>(combinedExpression, parameter) : x => true;
    }

    private Func<RouteConfig, object> BuildSortExpression(string? sort)
    {
        if (string.IsNullOrEmpty(sort))
        {
            return x => x.Id; // Default sorting
        }

        var property = typeof(RouteConfig).GetProperty(sort);
        return property == null ? throw new ArgumentException("Invalid sort property.") : (x => property.GetValue(x));
    }
}
