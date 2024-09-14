using AutoMapper;
using MediatR;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Application.Common.Interfaces.Repositories;
using Yarp.ReverseProxy.Configuration;
using Qorpe_Entities = Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Clusters.Commands.UpdateCluster;

public class UpdateClusterCommandHandler(
    IMapper mapper, IClusterRepository clusterRepository, IInMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<UpdateClusterCommand>
{
    public async Task Handle(UpdateClusterCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Qorpe_Entities.ClusterConfig>(request.Cluster);
        await clusterRepository.ReplaceOneAsync(entity);
        var immutableClusterConfig = mapper.Map<ClusterConfig>(entity);
        inMemoryConfigProvider.UpdateCluster(entity.ClusterId, immutableClusterConfig);
    }
}
