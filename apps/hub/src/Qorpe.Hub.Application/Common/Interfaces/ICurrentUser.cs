namespace Qorpe.Hub.Application.Common.Interfaces;

public interface ICurrentUser
{
    string? UserId { get; }
    string? Email { get; }
    string? Name { get; }
    bool IsAuthenticated { get; }

    bool HasClaim(string type, string value);
    string? FindClaim(string type);
}