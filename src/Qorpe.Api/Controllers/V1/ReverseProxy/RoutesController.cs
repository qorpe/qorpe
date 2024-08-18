using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Models;
using Qorpe.Application.Features.Routes.Commands.CreateRoute;
using Qorpe.Application.Features.Routes.Queries.GetRoutes;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("api/v{version:apiVersion}/routes")]
[ApiVersion("1.0")]
public class RoutesController : BaseController
{
    [HttpGet("{id}")]
    public IActionResult GetRoute(long id)
    {
        return Ok(id);
    }

    [HttpGet]
    public async Task<IActionResult> GetRoutes([FromQuery] GetRoutesQueryParameters queryParameters)
    {
        GetRoutesQuery query = new()
        {
            QueryParameters = queryParameters,
        };
        return Ok(await Mediator.Send(query));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoute([FromBody] RouteConfigDto body)
    {
        CreateRouteCommand command = new()
        {
            Route = body,
        };
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRoute(long id)
    {
        return Ok(id);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoute(long id)
    {
        return Ok(id);
    }
}
