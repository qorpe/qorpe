using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe_Entities = Qorpe.Domain.Entities;
using Qorpe.Application.Common.Interfaces;
using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Features.Routes.Commands.CreateRoute;

public class CreateRouteCommandHandler(
    IMapper mapper, IRouteRepository routeRepository, IInMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<CreateRouteCommand, RouteConfigDto>
{
    public async Task<RouteConfigDto> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Qorpe_Entities.RouteConfig>(request.Route);
        await routeRepository.InsertOneAsync(entity);
        request.Route.Id = entity?.Id;

        var immutableRouteConfig = mapper.Map<RouteConfig>(entity);
        inMemoryConfigProvider.AddRoute(immutableRouteConfig);

        return request.Route;
    }
}
