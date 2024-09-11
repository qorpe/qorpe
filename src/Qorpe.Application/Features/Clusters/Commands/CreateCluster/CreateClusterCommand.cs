using MediatR;
using Qorpe.Application.Common.DTOs;

namespace Qorpe.Application.Features.Clusters.Commands.CreateCluster;

public class CreateClusterCommand : IRequest<ClusterConfigDto>
{
    public required ClusterConfigDto Cluster { get; set; }
}