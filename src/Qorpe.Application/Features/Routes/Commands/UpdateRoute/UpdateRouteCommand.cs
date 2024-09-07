using MediatR;
using Qorpe.Application.Common.DTOs;

namespace Qorpe.Application.Features.Routes.Commands.UpdateRoute;

public class UpdateRouteCommand : IRequest
{
    public required RouteConfigDto Route { get; set; }
}
