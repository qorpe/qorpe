using MediatR;
using Qorpe.Application.Common.Interfaces.Repositories;
using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Features.Routes.Commands.DeleteRoute;

public class DeleteRouteCommandHandler(
    IRouteRepository routeRepository, InMemoryConfigProvider inMemoryConfigProvider) 
    : IRequestHandler<DeleteRouteCommand>
{
    public async Task Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        await routeRepository.DeleteByIdAsync(request.Id);
        RemoveRoute(request.RouteId);
    }

    public void RemoveRoute(string routeId)
    {
        var config = inMemoryConfigProvider.GetConfig();
        var currentRoutes = config.Routes.ToList();
        var routeIndex = currentRoutes.FindIndex(c => c.RouteId == routeId);
        if (routeIndex >= 0)
        {
            currentRoutes.RemoveAt(routeIndex);

            inMemoryConfigProvider.Update(currentRoutes, config.Clusters);
        }
    }
}
