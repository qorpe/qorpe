using MediatR;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Clusters.Commands.DeleteCluster;

public class DeleteClusterCommandHandler(IClusterRepository<ClusterConfig> clusterRepository) : IRequestHandler<DeleteClusterCommand>
{
    public async Task Handle(DeleteClusterCommand request, CancellationToken cancellationToken)
    {
        await clusterRepository.DeleteByIdAsync(request.Id);
    }
}
