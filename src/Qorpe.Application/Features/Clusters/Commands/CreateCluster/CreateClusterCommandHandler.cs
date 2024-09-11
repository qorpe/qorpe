using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Clusters.Commands.CreateCluster;

public class CreateClusterCommandHandler(IMapper mapper, IClusterRepository<ClusterConfig> clusterRepository) : IRequestHandler<CreateClusterCommand, ClusterConfigDto>
{
    public async Task<ClusterConfigDto> Handle(CreateClusterCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ClusterConfig>(request.Cluster);
        await clusterRepository.InsertOneAsync(entity);
        request.Cluster.Id = entity?.Id;
        return request.Cluster;
    }
}