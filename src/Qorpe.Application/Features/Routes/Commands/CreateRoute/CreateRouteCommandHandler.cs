using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe_Entities = Qorpe.Domain.Entities;
using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Features.Routes.Commands.CreateRoute;

public class CreateRouteCommandHandler(
    IMapper mapper, IRouteRepository routeRepository, InMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<CreateRouteCommand, RouteConfigDto>
{
    public async Task<RouteConfigDto> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Qorpe_Entities.RouteConfig>(request.Route);
        await routeRepository.InsertOneAsync(entity);
        request.Route.Id = entity?.Id;
        var immutableRouteConfig = mapper.Map<RouteConfig>(entity);
        AddRoute(immutableRouteConfig);
        return request.Route;
    }

    public void AddRoute(RouteConfig newRouteConfig)
    {
        var config = inMemoryConfigProvider.GetConfig();
        var currentRoutes = config.Routes.ToList();
        currentRoutes.Add(newRouteConfig);
        inMemoryConfigProvider.Update(currentRoutes, config.Clusters);
    }
}
