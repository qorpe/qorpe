using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.DTOs;
using Qorpe.Application.Common.Models;
using Qorpe.Application.Features.Clusters.Commands.CreateCluster;
using Qorpe.Application.Features.Clusters.Commands.DeleteCluster;
using Qorpe.Application.Features.Clusters.Commands.UpdateCluster;
using Qorpe.Application.Features.Clusters.Queries.GetCluster;
using Qorpe.Application.Features.Clusters.Queries.GetClusters;

namespace Qorpe.Api.Controllers.Proxy;

[Route("api/proxy/clusters")]
public class ClustersController : BaseController
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

    [HttpDelete]
    public async Task<IActionResult> DeleteCluster([FromQuery] string id, [FromQuery] string clusterId)
    {
        DeleteClusterCommand command = new()
        {
            Id = id,
            ClusterId = clusterId
        };
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCluster(string id)
    {
        GetClusterQuery query = new()
        {
            Id = id,
        };
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetClusters(
        [FromQuery] GetClustersQueryParameters queryParameters,
        [FromQuery] PaginationOptions paginationOptions,
        [FromQuery] bool fromMemory)
    {
        GetClustersQuery query = new()
        {
            QueryParameters = queryParameters,
            PaginationOptions = paginationOptions,
            FromMemory = fromMemory
        };
        var response = await Mediator.Send(query);
        return Ok(response);
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
