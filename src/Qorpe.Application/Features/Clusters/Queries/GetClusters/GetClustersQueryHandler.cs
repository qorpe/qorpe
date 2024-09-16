using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Helpers;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Application.Common.Models;
using Qorpe.Domain.Entities;
using Yarp_Configuration = Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Features.Clusters.Queries.GetClusters;

public class GetClustersQueryHandler(IMapper mapper, IClusterRepository clusterRepository)
    : IRequestHandler<GetClustersQuery, PaginatedResponse<ClusterConfigDto>>
{
    public async Task<PaginatedResponse<ClusterConfigDto>> Handle(GetClustersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await GetFromDatabase(request);
        //if (request.FromMemory)
        //{
        //    return await GetFromMemory(request);
        //}
        //else
        //{
        //    return await GetFromDatabase(request);
        //}
    }

    //private async Task<PaginatedResponse<ClusterConfigDto>> GetFromMemory(GetClustersQuery request)
    //{
    //    if (string.IsNullOrEmpty(request.PaginationOptions.SortBy) || 
    //        request.PaginationOptions.SortBy == "CreatedAt") 
    //    {
    //        request.PaginationOptions.SortBy = "ClusterId";
    //    }

    //    var filterExpression = ExpressionHelper.BuildFilterExpression<GetClustersQueryParameters, Yarp_Configuration.ClusterConfig>(request.QueryParameters);
    //    var compiledFilter = filterExpression.Compile();
    //    var clusterConfigs = inMemoryConfigProvider.GetConfig().Clusters.ToList();

    //    var clusters = ListHelper.ApplyFilteringSortingAndPagination(
    //        clusterConfigs,
    //        compiledFilter,
    //        sortBy: request.PaginationOptions.SortBy,
    //        IsAscending: request.PaginationOptions.IsAscending,
    //        pageNumber: request.PaginationOptions.Page,
    //        pageSize: request.PaginationOptions.PageSize
    //    );

    //    var totalCount = clusterConfigs.Where(compiledFilter).Count();

    //    var response = new PaginatedResponse<ClusterConfigDto>(mapper.Map<List<ClusterConfigDto>>(clusters),
    //                                                           totalCount,
    //                                                           request.PaginationOptions.Page,
    //                                                           request.PaginationOptions.PageSize);

    //    return await Task.FromResult(response);
    //}

    private async Task<PaginatedResponse<ClusterConfigDto>> GetFromDatabase(GetClustersQuery request)
    {
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
