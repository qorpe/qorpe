using AutoMapper;
using MediatR;
using Qorpe.Application.Common.Interfaces.Repositories;
using Yarp.ReverseProxy.Configuration;
using Qorpe_Entities = Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Routes.Commands.UpdateRoute;

public class UpdateRouteCommandHandler(
    IMapper mapper, IRouteRepository routeRepository, InMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<UpdateRouteCommand>
{
    public async Task Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Qorpe_Entities.RouteConfig>(request.Route);
        await routeRepository.ReplaceOneAsync(entity);
        var immutableRouteConfig = mapper.Map<RouteConfig>(entity);
        UpdateRoute(entity.RouteId, immutableRouteConfig);
    }

    public void UpdateRoute(string routeId, RouteConfig newRouteConfig)
    {
        var config = inMemoryConfigProvider.GetConfig();
        var currentRoutes = config.Routes.ToList();
        var routeIndex = currentRoutes.FindIndex(r => r.RouteId == routeId);
        if (routeIndex >= 0)
        {
            currentRoutes[routeIndex] = newRouteConfig;
            inMemoryConfigProvider.Update(currentRoutes, config.Clusters);
        }
    }
}
