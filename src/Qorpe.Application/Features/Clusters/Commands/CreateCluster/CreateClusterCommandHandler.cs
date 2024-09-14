using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Application.Common.Interfaces.Repositories;
using Yarp.ReverseProxy.Configuration;
using Qorpe_Entities = Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Clusters.Commands.CreateCluster;

public class CreateClusterCommandHandler(
    IMapper mapper, IClusterRepository clusterRepository, IInMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<CreateClusterCommand, ClusterConfigDto>
{
    public async Task<ClusterConfigDto> Handle(CreateClusterCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Qorpe_Entities.ClusterConfig>(request.Cluster);
        await clusterRepository.InsertOneAsync(entity);
        request.Cluster.Id = entity?.Id;
        var immutableClusterConfig = mapper.Map<ClusterConfig>(entity);
        inMemoryConfigProvider.AddCluster(immutableClusterConfig);
        return request.Cluster;
    }
}