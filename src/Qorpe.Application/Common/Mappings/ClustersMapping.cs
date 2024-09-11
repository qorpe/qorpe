using AutoMapper;
using Qorpe.Application.Common.DTOs;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Common.Mappings;

public class ClustersMapping : Profile
{
    public ClustersMapping()
    {
        CreateMap<ClusterConfig, ClusterConfigDto>();
        CreateMap<SessionAffinityConfig, SessionAffinityConfigDto>();
        CreateMap<HealthCheckConfig, HealthCheckConfigDto>();
        CreateMap<HttpClientConfig, HttpClientConfigDto>();
        CreateMap<ForwarderRequestConfig, ForwarderRequestConfigDto>();
        CreateMap<DestinationConfig, DestinationConfigDto>();
        CreateMap<SessionAffinityCookieConfig, SessionAffinityCookieConfigDto>();
        CreateMap<ActiveHealthCheckConfig, ActiveHealthCheckConfigDto>();
        CreateMap<PassiveHealthCheckConfig, PassiveHealthCheckConfigDto>();
        CreateMap<WebProxyConfig, WebProxyConfigDto>();

        CreateMap<ClusterConfigDto, ClusterConfig>();
        CreateMap<SessionAffinityConfigDto, SessionAffinityConfig>();
        CreateMap<HealthCheckConfigDto, HealthCheckConfig>();
        CreateMap<HttpClientConfigDto, HttpClientConfig>();
        CreateMap<ForwarderRequestConfigDto, ForwarderRequestConfig>();
        CreateMap<DestinationConfigDto, DestinationConfig>();
        CreateMap<SessionAffinityCookieConfigDto, SessionAffinityCookieConfig>();
        CreateMap<ActiveHealthCheckConfigDto, ActiveHealthCheckConfig>();
        CreateMap<PassiveHealthCheckConfigDto, PassiveHealthCheckConfig>();
        CreateMap<WebProxyConfigDto, WebProxyConfig>();
    }
}
