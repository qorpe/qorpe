using Microsoft.Extensions.DependencyInjection;
using Qorpe.Hub.SDK.Clients;
using Refit;

namespace Qorpe.Hub.SDK.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHubTenantsClient(this IServiceCollection services, Uri hubBaseAddress)
    {
        services.AddRefitClient<ITenantsClient>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = hubBaseAddress;
                c.Timeout = Timeout.InfiniteTimeSpan;
            });

        return services;
    }
}