using Qorpe.Domain.Common;

namespace Qorpe.Application.Features.Clusters.Queries.GetClusters;

public class GetClustersQueryParameters : Document
{
    public string? ClusterId { get; set; }
    public string? LoadBalancingPolicy { get; set; }
}
