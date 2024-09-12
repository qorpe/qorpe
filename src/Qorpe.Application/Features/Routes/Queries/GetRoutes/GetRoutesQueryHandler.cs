using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Helpers;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Application.Common.Models;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Routes.Queries.GetRoutes;

public class GetRoutesQueryHandler(IMapper mapper, IRouteRepository routeRepository) 
    : IRequestHandler<GetRoutesQuery, PaginatedResponse<RouteConfigDto>>
{
    public async Task<PaginatedResponse<RouteConfigDto>> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

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
