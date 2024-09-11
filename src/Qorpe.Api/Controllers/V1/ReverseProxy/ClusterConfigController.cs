using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Features.Clusters.Commands.CreateCluster;
using Qorpe.Application.Features.Clusters.Commands.DeleteCluster;
using Qorpe.Application.Features.Clusters.Commands.UpdateCluster;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("api/v{version:apiVersion}/clusters")]
[ApiVersion("1.0")]
public class ClusterConfigController : BaseController
{
    #region Cluster(s)
    [HttpPost]
    public async Task<IActionResult> CreateCluster([FromBody] ClusterConfigDto body)
    {
        CreateClusterCommand command = new()
        {
            Cluster = body,
        };
        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCluster(string id)
    {
        DeleteClusterCommand command = new()
        {
            Id = id,
        };
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCluster([FromBody] ClusterConfigDto body)
    {
        UpdateClusterCommand command = new()
        {
            Cluster = body,
        };
        await Mediator.Send(command);
        return Ok();
    }
    #endregion
}
