using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("Qorpe.Api/V{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ClusterConfigController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}
