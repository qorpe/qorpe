using AutoMapper;
using MediatR;
using Qorpe.Application.Common.Interfaces.Repositories;
using Yarp.ReverseProxy.Configuration;
using Qorpe_Entities = Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Clusters.Commands.UpdateCluster;

public class UpdateClusterCommandHandler(
    IMapper mapper, IClusterRepository clusterRepository, InMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<UpdateClusterCommand>
{
    public async Task Handle(UpdateClusterCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Qorpe_Entities.ClusterConfig>(request.Cluster);
        await clusterRepository.ReplaceOneAsync(entity);
        var immutableClusterConfig = mapper.Map<ClusterConfig>(entity);
        UpdateCluster(entity.ClusterId, immutableClusterConfig);
    }

    public void UpdateCluster(string clusterId, ClusterConfig newClusterConfig)
    {
        var config = inMemoryConfigProvider.GetConfig();
        var currentClusters = config.Clusters.ToList();
        var clusterIndex = currentClusters.FindIndex(c => c.ClusterId == clusterId);
        if (clusterIndex >= 0)
        {
            currentClusters[clusterIndex] = newClusterConfig;
            inMemoryConfigProvider.Update(config.Routes, currentClusters);
        }
    }
}
