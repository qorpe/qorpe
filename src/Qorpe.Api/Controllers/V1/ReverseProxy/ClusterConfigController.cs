using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;
using System.Linq.Expressions;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("Qorpe.Api/V{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ClusterConfigController(IRepositoryFactory repositoryFactory) : BaseController
{
    [HttpPost]
    public IActionResult Index([FromBody] Expression<Func<RouteConfig, bool>> filterExpression)
    {
        IRepository<RouteConfig> a = repositoryFactory.CreateRepository<RouteConfig>("commone");

        var b = a.FindOne(filterExpression);

        return Ok(b);
    }
}

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

class Customer
{
    public int CustomerId { get; set; }
    public string? Name { get; set; }
}