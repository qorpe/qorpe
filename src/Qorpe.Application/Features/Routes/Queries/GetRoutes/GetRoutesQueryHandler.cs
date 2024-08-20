using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Extensions;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Application.Common.Models;
using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Features.Routes.Queries.GetRoutes;

public class GetRoutesQueryHandler(IApplicationDbContext context, InMemoryConfigProvider configProvider, IMapper mapper) : IRequestHandler<GetRoutesQuery, PaginationList<RouteConfigDto>>
{
    public async Task<PaginationList<RouteConfigDto>> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.QueryParameters);

        try
        {
            bool? fromCache = request.QueryParameters.FromCache;

            if (fromCache.HasValue)
            {
                if (fromCache is true)
                {
                    var result = await FromCache(request);
                    return result;
                }
                else
                {
                    var result = await FromDatabase(request);
                    return result;
                }
            }
            else
            {
                var result = await FromCache(request);
                result ??= await FromDatabase(request);
                return result;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    private async Task<PaginationList<RouteConfigDto>> FromDatabase(GetRoutesQuery request)
    {
        int pageNumber = request.QueryParameters.PageNumber;
        int pageSize = request.QueryParameters.PageSize;

        var query = context.RouteConfigs.AsQueryable();

        if (request.QueryParameters.Id.HasValue)
        {
            query = query.Where(item => item.Id == request.QueryParameters.Id);
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.RouteId))
        {
            query = query.Where(item => item.RouteId == request.QueryParameters.RouteId);
        }

        if (request.QueryParameters.Order.HasValue)
        {
            query = query.Where(item => item.Order == request.QueryParameters.Order);
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.ClusterId))
        {
            query = query.Where(item => item.ClusterId == request.QueryParameters.ClusterId);
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.AuthorizationPolicy))
        {
            query = query.Where(item => item.AuthorizationPolicy == request.QueryParameters.AuthorizationPolicy);
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.RateLimiterPolicy))
        {
            query = query.Where(item => item.RateLimiterPolicy == request.QueryParameters.RateLimiterPolicy);
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.OutputCachePolicy))
        {
            query = query.Where(item => item.OutputCachePolicy == request.QueryParameters.OutputCachePolicy);
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.TimeoutPolicy))
        {
            query = query.Where(item => item.TimeoutPolicy == request.QueryParameters.TimeoutPolicy);
        }

        //if (!string.IsNullOrEmpty(request.QueryParameters.Timeout))
        //{
        //    query = query.Where(item => item.Timeout == request.QueryParameters.Timeout.To);
        //}

        if (!string.IsNullOrEmpty(request.QueryParameters.CorsPolicy))
        {
            query = query.Where(item => item.CorsPolicy == request.QueryParameters.CorsPolicy);
        }

        if (request.QueryParameters.MaxRequestBodySize.HasValue)
        {
            query = query.Where(item => item.MaxRequestBodySize == request.QueryParameters.MaxRequestBodySize);
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.SortBy))
        {
            if (request.QueryParameters.SortOrder == "desc")
            {
                query = query.OrderByDescending(item => EF.Property<object>(item, request.QueryParameters.SortBy));
            }
            else
            {
                query = query.OrderBy(item => EF.Property<object>(item, request.QueryParameters.SortBy));
            }
        }

        return await query.ProjectTo<RouteConfigDto>(mapper.ConfigurationProvider)
                          .PaginatedListAsync(pageNumber, pageSize);
    }

    private async Task<PaginationList<RouteConfigDto>> FromCache(GetRoutesQuery request)
    {
        int pageNumber = request.QueryParameters.PageNumber;
        int pageSize = request.QueryParameters.PageSize;

        List<RouteConfig> routes = [.. configProvider.GetConfig().Routes];

        if (!string.IsNullOrEmpty(request.QueryParameters.RouteId))
        {
            routes = [.. routes.Where(item => item.RouteId == request.QueryParameters.RouteId)];
        }

        if (request.QueryParameters.Order.HasValue)
        {
            routes = [.. routes.Where(item => item.Order == request.QueryParameters.Order)];
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.ClusterId))
        {
            routes = [.. routes.Where(item => item.ClusterId == request.QueryParameters.ClusterId)];
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.AuthorizationPolicy))
        {
            routes = [.. routes.Where(item => item.AuthorizationPolicy == request.QueryParameters.AuthorizationPolicy)];
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.RateLimiterPolicy))
        {
            routes = [.. routes.Where(item => item.RateLimiterPolicy == request.QueryParameters.RateLimiterPolicy)];
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.TimeoutPolicy))
        {
            routes = [.. routes.Where(item => item.TimeoutPolicy == request.QueryParameters.TimeoutPolicy)];
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.CorsPolicy))
        {
            routes = [.. routes.Where(item => item.CorsPolicy == request.QueryParameters.CorsPolicy)];
        }

        if (request.QueryParameters.MaxRequestBodySize.HasValue)
        {
            routes = [.. routes.Where(item => item.MaxRequestBodySize == request.QueryParameters.MaxRequestBodySize)];
        }

        if (!string.IsNullOrEmpty(request.QueryParameters.SortBy))
        {
            if (request.QueryParameters.SortOrder == "desc")
            {
                routes = [.. routes.OrderByDescending(item => EF.Property<object>(item, request.QueryParameters.SortBy))];
            }
            else
            {
                routes = [.. routes.OrderBy(item => EF.Property<object>(item, request.QueryParameters.SortBy))];
            }
        }

        var mappedRoutes = mapper.Map<List<RouteConfigDto>>(routes);
        PaginationList<RouteConfigDto> result = new(mappedRoutes, 1, pageNumber, pageSize);
        return result;
    }
}
