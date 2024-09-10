using MediatR;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Models;

namespace Qorpe.Application.Features.Routes.Queries.GetRoutes;

public class GetRoutesQuery : IRequest<PaginatedResponse<RouteConfigDto>>
{
    public GetRoutesQueryParameters QueryParameters { get; set; } = default!;
    public PaginationOptions PaginationOptions { get; set; } = default!;
}
