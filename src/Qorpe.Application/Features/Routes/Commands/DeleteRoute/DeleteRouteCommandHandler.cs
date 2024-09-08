using MediatR;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Routes.Commands.DeleteRoute;

public class DeleteRouteCommandHandler(IRouteRepository<RouteConfig> routeRepository) : IRequestHandler<DeleteRouteCommand>
{
    public async Task Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        await routeRepository.DeleteByIdAsync(request.Id);
    }
}
