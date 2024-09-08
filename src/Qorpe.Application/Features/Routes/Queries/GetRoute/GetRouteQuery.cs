using MediatR;
using Qorpe.Application.Common.DTOs;

namespace Qorpe.Application.Features.Routes.Queries.GetRoute;

public class GetRouteQuery : IRequest<RouteConfigDto>
{
    public required string Id { get; set; }
}
