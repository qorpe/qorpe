using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Features.Routes.Commands.CreateRoute;
using Qorpe.Application.Features.Routes.Commands.UpdateRoute;
using Qorpe.Application.Features.Routes.Queries.GetRoutes;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("api/v{version:apiVersion}/routes")]
[ApiVersion("1.0")]
public class RoutesController : BaseController
{
    #region Route(s)
    [HttpGet("{id}")]
    public IActionResult GetRoute(string id)
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
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoute([FromBody] RouteConfigDto body)
    {
        CreateRouteCommand command = new()
        {
            Route = body,
        };
        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoute([FromBody] RouteConfigDto body)
    {
        UpdateRouteCommand command = new()
        {
            Route = body,
        };
        await Mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoute(string id)
    {
        return Ok(id);
    }
    #endregion
}
