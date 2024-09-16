using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Models;
using Qorpe.Application.Features.Routes.Commands.CreateRoute;
using Qorpe.Application.Features.Routes.Commands.DeleteRoute;
using Qorpe.Application.Features.Routes.Commands.UpdateRoute;
using Qorpe.Application.Features.Routes.Queries.GetRoute;
using Qorpe.Application.Features.Routes.Queries.GetRoutes;
using System.Diagnostics;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("api/routes")]
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

    [HttpPost("LoadData")]
    public async Task<IActionResult> CreateRoutes([FromQuery] int size, [FromBody] RouteConfigDto body)
    {
        // Create a new Stopwatch instance
        Stopwatch stopwatch = new();

        // Start measuring time
        stopwatch.Start();

        for (int i = 0; i < size; i++)
        {
            body.Id = null;
            body.RouteId = body.RouteId + i;
            body.ClusterId = body.ClusterId + i;
            CreateRouteCommand command = new()
            {
                Route = body,
            };
            await Mediator.Send(command);
        }

        // Stop measuring time
        stopwatch.Stop();

        // Convert elapsed time to string
        string elapsedTimeString = FormatElapsedTime(stopwatch.Elapsed);

        return Ok(elapsedTimeString);
    }

    static string FormatElapsedTime(TimeSpan elapsedTime)
    {
        // Format elapsed time as "hh:mm:ss:fff"
        return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
            elapsedTime.Hours,
            elapsedTime.Minutes,
            elapsedTime.Seconds,
            elapsedTime.Milliseconds);
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
