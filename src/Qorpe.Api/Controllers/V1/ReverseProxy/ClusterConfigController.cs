using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Application.Features.Clusters.Commands.CreateCluster;
using Qorpe.Domain.Entities;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("api/v{version:apiVersion}/clusters")]
[ApiVersion("1.0")]
public class ClusterConfigController(IRouteRepository<RouteConfig> routeRepository) : BaseController
{
    #region Cluster(s)
    [HttpPost]
    public async Task<IActionResult> CreateRoute([FromBody] ClusterConfigDto body)
    {
        CreateClusterCommand command = new()
        {
            Cluster = body,
        };
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    #endregion
}
