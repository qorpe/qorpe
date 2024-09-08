using AutoMapper;
using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Features.Routes.Queries.GetRoute;

public class GetRouteQueryHandler(IMapper mapper, IRouteRepository<RouteConfig> routeRepository) 
    : IRequestHandler<GetRouteQuery, RouteConfigDto>
{
    public async Task<RouteConfigDto> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        var routeConfig = await routeRepository.FindByIdAsync(request.Id);

        return routeConfig is null 
            ? throw new KeyNotFoundException($"RouteConfig with Id {request.Id} was not found.") // Todo - Consider Custom Exception
            : mapper.Map<RouteConfigDto>(routeConfig);
    }
}
