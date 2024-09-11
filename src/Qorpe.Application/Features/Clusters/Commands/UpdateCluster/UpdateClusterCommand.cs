using MediatR;
using Qorpe.Application.Common.DTOs;

namespace Qorpe.Application.Features.Clusters.Commands.UpdateCluster;

public class UpdateClusterCommand : IRequest
{
    public required ClusterConfigDto Cluster { get; set; }
}
