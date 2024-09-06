using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Domain.Entities.Lite;

namespace Qorpe.Application.Features.Routes.Commands.CreateRoute;

public class CreateRouteCommandHandler(IMapper mapper) : IRequestHandler<CreateRouteCommand, RouteConfigDto>
{
    public async Task<RouteConfigDto> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ArgumentNullException.ThrowIfNull(request.Route);

        var entity = mapper.Map<RouteConfig>(request.Route);

        return request.Route;
    }
}
