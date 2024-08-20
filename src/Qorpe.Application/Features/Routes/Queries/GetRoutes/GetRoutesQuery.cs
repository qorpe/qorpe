using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Models;

namespace Qorpe.Application.Features.Routes.Queries.GetRoutes;

public class GetRoutesQuery : IRequest<PaginationList<RouteConfigDto>>
{
    public required GetRoutesQueryParameters QueryParameters { get; set; }
}
