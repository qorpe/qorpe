using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Models;

namespace Qorpe.Application.Features.Clusters.Queries.GetClusters;

public class GetClustersQuery : IRequest<PaginatedResponse<ClusterConfigDto>>
{
    public GetClustersQueryParameters QueryParameters { get; set; } = default!;
    public PaginationOptions PaginationOptions { get; set; } = default!;
}
