using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetRoutes()
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult CreateRoute()
    {
        return Ok();
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
