using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Qorpe.Hub.Application.Common.Interfaces;

namespace Qorpe.Hub.Infrastructure.Auth;

public class CurrentUser(IHttpContextAccessor http) : ICurrentUser
{
    private ClaimsPrincipal? Principal => http.HttpContext?.User;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier) 
                             ?? Principal?.FindFirstValue("sub");

    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email);
    public string? Name  => Principal?.Identity?.Name;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

    public bool HasClaim(string type, string value)
        => Principal?.HasClaim(type, value) == true;

    public string? FindClaim(string type)
        => Principal?.FindFirstValue(type);
}