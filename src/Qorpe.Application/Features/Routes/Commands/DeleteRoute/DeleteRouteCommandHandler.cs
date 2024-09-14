using MediatR;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Application.Common.Interfaces.Repositories;

namespace Qorpe.Application.Features.Routes.Commands.DeleteRoute;

public class DeleteRouteCommandHandler(IRouteRepository routeRepository, IInMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<DeleteRouteCommand>
{
    public async Task Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        await routeRepository.DeleteByIdAsync(request.Id);
        inMemoryConfigProvider.RemoveRoute(request.RouteId);
    }
}
