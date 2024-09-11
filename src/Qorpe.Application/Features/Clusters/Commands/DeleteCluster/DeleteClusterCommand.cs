using MediatR;

namespace Qorpe.Application.Features.Clusters.Commands.DeleteCluster;

public class DeleteClusterCommand : IRequest
{
    public required string Id { get; set; }
}
