using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces.Repositories;

namespace Qorpe.Application.Features.Clusters.Queries.GetCluster;

public class GetClusterQueryHandler(IMapper mapper, IClusterRepository clusterRepository)
    : IRequestHandler<GetClusterQuery, ClusterConfigDto>
{
    public async Task<ClusterConfigDto> Handle(GetClusterQuery request, CancellationToken cancellationToken)
    {
        var clusterConfig = await clusterRepository.FindByIdAsync(request.Id);

        return clusterConfig is null
            ? throw new KeyNotFoundException($"ClusterConfig with Id {request.Id} was not found.") // Todo - Consider Custom Exception
            : mapper.Map<ClusterConfigDto>(clusterConfig);
    }
}
