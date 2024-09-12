using AutoMapper;
using MediatR;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Clusters.Commands.UpdateCluster;

public class UpdateClusterCommandHandler(IMapper mapper, IClusterRepository clusterRepository) : IRequestHandler<UpdateClusterCommand>
{
    public async Task Handle(UpdateClusterCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ClusterConfig>(request.Cluster);
        await clusterRepository.ReplaceOneAsync(entity);
    }
}
