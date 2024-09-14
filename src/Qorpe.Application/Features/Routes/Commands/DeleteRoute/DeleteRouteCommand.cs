using MediatR;

namespace Qorpe.Application.Features.Routes.Commands.DeleteRoute;

public class DeleteRouteCommand : IRequest
{
    public required string Id { get; set; }
    public required string RouteId { get; set; }
}
