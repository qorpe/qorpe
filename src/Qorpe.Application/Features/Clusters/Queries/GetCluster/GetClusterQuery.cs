using MediatR;
using Qorpe.Application.Common.DTOs;

namespace Qorpe.Application.Features.Clusters.Queries.GetCluster;

public class GetClusterQuery : IRequest<ClusterConfigDto>
{
    public required string Id { get; set; }
}
