using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Helpers;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Application.Common.Models;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Clusters.Queries.GetClusters;

public class GetClustersQueryHandler(IMapper mapper, IClusterRepository clusterRepository)
    : IRequestHandler<GetClustersQuery, PaginatedResponse<ClusterConfigDto>>
{
    public async Task<PaginatedResponse<ClusterConfigDto>> Handle(GetClustersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var filterExpression = ExpressionHelper.BuildFilterExpression<GetClustersQueryParameters, ClusterConfig>(request.QueryParameters);

        var clusters = await clusterRepository.FilterByAsync(filterExpression,
                                                             request.PaginationOptions.Page,
                                                             request.PaginationOptions.PageSize,
                                                             request.PaginationOptions.SortBy,
                                                             request.PaginationOptions.IsAscending);

        var totalCount = await clusterRepository.CountAsync(filterExpression);

        var response = new PaginatedResponse<ClusterConfigDto>(mapper.Map<List<ClusterConfigDto>>(clusters),
                                                               totalCount,
                                                               request.PaginationOptions.Page,
                                                               request.PaginationOptions.PageSize);

        return response;
    }
}
