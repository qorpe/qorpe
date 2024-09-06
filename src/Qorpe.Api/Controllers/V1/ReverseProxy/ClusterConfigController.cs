using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("Qorpe.Api/V{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ClusterConfigController(IRouteRepository<RouteConfig> routeRepository) : BaseController
{
    [HttpPost]
    public IActionResult Index([FromQuery] string id)
    {
        var a = routeRepository.FindById(id);
        return Ok(a);
    }
}
