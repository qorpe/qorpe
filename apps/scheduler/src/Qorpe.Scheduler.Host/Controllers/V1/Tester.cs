using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Qorpe.Scheduler.Host.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("t/{tenant}/scheduler/v{version:apiVersion}/tester")]
[Authorize(Policy = "TenantMatch")]
public class Tester : ControllerBase
{
    // GET
    [HttpGet("Commone")]
    public IActionResult Index(string tenant)
    {
        return Ok("Geliyo geliyor oooo ürünüm geliyooo!");
    }
}