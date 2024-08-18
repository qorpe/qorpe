using MediatR;
using Qorpe.Application.Common.DTOs;

namespace Qorpe.Application.Features.Routes.Commands.CreateRoute;

public class CreateRouteCommand : IRequest<RouteConfigDto>
{
    public RouteConfigDto? Route { get; set; }
}
