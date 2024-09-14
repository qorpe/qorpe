using AutoMapper;
using MediatR;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Application.Common.Interfaces.Repositories;
using Yarp.ReverseProxy.Configuration;
using Qorpe_Entities = Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Routes.Commands.UpdateRoute;

public class UpdateRouteCommandHandler(
    IMapper mapper, IRouteRepository routeRepository, IInMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<UpdateRouteCommand>
{
    public async Task Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Qorpe_Entities.RouteConfig>(request.Route);
        await routeRepository.ReplaceOneAsync(entity);
        var immutableRouteConfig = mapper.Map<RouteConfig>(entity);
        inMemoryConfigProvider.UpdateRoute(entity.RouteId, immutableRouteConfig);
    }
}
