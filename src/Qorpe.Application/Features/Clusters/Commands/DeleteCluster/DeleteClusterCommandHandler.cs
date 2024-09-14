using MediatR;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Application.Common.Interfaces.Repositories;

namespace Qorpe.Application.Features.Clusters.Commands.DeleteCluster;

public class DeleteClusterCommandHandler(
    IClusterRepository clusterRepository, IInMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<DeleteClusterCommand>
{
    public async Task Handle(DeleteClusterCommand request, CancellationToken cancellationToken)
    {
        await clusterRepository.DeleteByIdAsync(request.Id);
        inMemoryConfigProvider.RemoveCluster(request.ClusterId);
    }
}
