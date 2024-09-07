using AutoMapper;
using MediatR;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Routes.Commands.UpdateRoute;

public class UpdateRouteCommandHandler(IMapper mapper, IRouteRepository<RouteConfig> routeRepository) : IRequestHandler<UpdateRouteCommand>
{
    public async Task Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<RouteConfig>(request.Route);
        await routeRepository.ReplaceOneAsync(entity);
    }
}
