using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Models;
using Qorpe.Application.Features.Routes.Commands.CreateRoute;
using Qorpe.Application.Features.Routes.Commands.DeleteRoute;
using Qorpe.Application.Features.Routes.Commands.UpdateRoute;
using Qorpe.Application.Features.Routes.Queries.GetRoute;
using Qorpe.Application.Features.Routes.Queries.GetRoutes;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("api/v{version:apiVersion}/routes")]
[ApiVersion("1.0")]
public class RoutesController : BaseController
{
    #region Route(s)
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

    [HttpDelete]
    public async Task<IActionResult> DeleteRoute([FromQuery] string id, [FromQuery] string routeId)
    {
        DeleteRouteCommand command = new()
        {
            Id = id,
            RouteId = routeId
        };
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoute(string id)
    {
        GetRouteQuery query = new()
        {
            Id = id,
        };
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetRoutes(
        [FromQuery] GetRoutesQueryParameters queryParameters, 
        [FromQuery] PaginationOptions paginationOptions,
        [FromQuery] bool fromMemory)
    {
        GetRoutesQuery query = new()
        {
            QueryParameters = queryParameters,
            PaginationOptions = paginationOptions,
            FromMemory = fromMemory
        };
        var response = await Mediator.Send(query);
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
    #endregion
}
