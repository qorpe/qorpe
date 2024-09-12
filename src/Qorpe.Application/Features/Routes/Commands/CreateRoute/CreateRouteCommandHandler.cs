using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Routes.Commands.CreateRoute;


public class CreateRouteCommandHandler(IMapper mapper, IRouteRepository routeRepository) : IRequestHandler<CreateRouteCommand, RouteConfigDto>
{
    public async Task<RouteConfigDto> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<RouteConfig>(request.Route);
        await routeRepository.InsertOneAsync(entity);
        request.Route.Id = entity?.Id;
        return request.Route;
    }
}
