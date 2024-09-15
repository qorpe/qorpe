using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Helpers;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Application.Common.Models;
using Qorpe.Domain.Entities;
using Yarp_Configuration = Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Features.Routes.Queries.GetRoutes;

public class GetRoutesQueryHandler(
    IMapper mapper, IRouteRepository routeRepository, IInMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<GetRoutesQuery, PaginatedResponse<RouteConfigDto>>
{
    public async Task<PaginatedResponse<RouteConfigDto>> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.FromMemory)
        {
            return await GetFromMemory(request);
        }
        else
        {
            return await GetFromDatabase(request);
        }
    }

    private async Task<PaginatedResponse<RouteConfigDto>> GetFromMemory(GetRoutesQuery request)
    {
        if (string.IsNullOrEmpty(request.PaginationOptions.SortBy) ||
            request.PaginationOptions.SortBy == "CreatedAt")
        {
            request.PaginationOptions.SortBy = "RouteId";
        }

        var filterExpression = ExpressionHelper.BuildFilterExpression<GetRoutesQueryParameters, Yarp_Configuration.RouteConfig>(request.QueryParameters);
        var compiledFilter = filterExpression.Compile();
        var routeConfigs = inMemoryConfigProvider.GetConfig().Routes.ToList();

        var routes = ListHelper.ApplyFilteringSortingAndPagination(
            routeConfigs,
            compiledFilter,
            sortBy: request.PaginationOptions.SortBy,
            IsAscending: request.PaginationOptions.IsAscending,
            pageNumber: request.PaginationOptions.Page,
            pageSize: request.PaginationOptions.PageSize
        );

        var totalCount = routeConfigs.Where(compiledFilter).Count();

        var response = new PaginatedResponse<RouteConfigDto>(mapper.Map<List<RouteConfigDto>>(routes),
                                                             totalCount,
                                                             request.PaginationOptions.Page,
                                                             request.PaginationOptions.PageSize);

        return await Task.FromResult(response);
    }

    private async Task<PaginatedResponse<RouteConfigDto>> GetFromDatabase(GetRoutesQuery request)
    {
        var filterExpression = ExpressionHelper.BuildFilterExpression<GetRoutesQueryParameters, RouteConfig>(request.QueryParameters);

        var routes = await routeRepository.FilterByAsync(filterExpression,
                                                         request.PaginationOptions.Page,
                                                         request.PaginationOptions.PageSize,
                                                         request.PaginationOptions.SortBy,
                                                         request.PaginationOptions.IsAscending);

        var totalCount = await routeRepository.CountAsync(filterExpression);

        var response = new PaginatedResponse<RouteConfigDto>(mapper.Map<List<RouteConfigDto>>(routes),
                                                             totalCount,
                                                             request.PaginationOptions.Page,
                                                             request.PaginationOptions.PageSize);

        return response;
    }
}
