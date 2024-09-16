using MediatR;
using Qorpe.Application.Common.Interfaces.Repositories;
using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Features.Clusters.Commands.DeleteCluster;

public class DeleteClusterCommandHandler(
    IClusterRepository clusterRepository, InMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<DeleteClusterCommand>
{
    public async Task Handle(DeleteClusterCommand request, CancellationToken cancellationToken)
    {
        await clusterRepository.DeleteByIdAsync(request.Id);
        RemoveCluster(request.ClusterId);
    }

    public void RemoveCluster(string clusterId)
    {
        var config = inMemoryConfigProvider.GetConfig();
        var currentClusters = config.Clusters.ToList();
        var clusterIndex = currentClusters.FindIndex(c => c.ClusterId == clusterId);
        if (clusterIndex >= 0)
        {
            currentClusters.RemoveAt(clusterIndex);
            inMemoryConfigProvider.Update(config.Routes, currentClusters);
        }
    }
}
