using MediatR;
using Qorpe.Application.Common.DTOs;

namespace Qorpe.Application.Features.Routes.Commands.CreateRoute;

public class CreateRouteCommand : IRequest<RouteConfigDto>
{
    public required RouteConfigDto Route { get; set; }
}
