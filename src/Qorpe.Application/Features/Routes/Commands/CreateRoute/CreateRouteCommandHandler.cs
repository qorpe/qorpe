using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Routes.Commands.CreateRoute;

public class CreateRouteCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CreateRouteCommand, RouteConfigDto>
{
    public async Task<RouteConfigDto> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ArgumentNullException.ThrowIfNull(request.Route);

        var entity = mapper.Map<RouteConfig>(request.Route);
        context.RouteConfigs.Add(entity);
        long id = await context.SaveChangesAsync(cancellationToken);
        return request.Route;
    }
}
