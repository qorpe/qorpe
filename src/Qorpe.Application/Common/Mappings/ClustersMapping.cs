using AutoMapper;
using Qorpe.Application.Common.DTOs;
using Qorpe.Domain.Entities;
using Yarp_Configuration = Yarp.ReverseProxy.Configuration;
using Yarp_Forwarder = Yarp.ReverseProxy.Forwarder;

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

        CreateMap<Yarp_Configuration.ClusterConfig, ClusterConfig>();
        CreateMap<Yarp_Configuration.SessionAffinityConfig, SessionAffinityConfig>();
        CreateMap<Yarp_Configuration.HealthCheckConfig, HealthCheckConfig>();
        CreateMap<Yarp_Configuration.HttpClientConfig, HttpClientConfig>();
        CreateMap<Yarp_Forwarder.ForwarderRequestConfig, ForwarderRequestConfig>();
        CreateMap<Yarp_Configuration.DestinationConfig, DestinationConfig>();
        CreateMap<Yarp_Configuration.SessionAffinityCookieConfig, SessionAffinityCookieConfig>();
        CreateMap<Yarp_Configuration.ActiveHealthCheckConfig, ActiveHealthCheckConfig>();
        CreateMap<Yarp_Configuration.PassiveHealthCheckConfig, PassiveHealthCheckConfig>();
        CreateMap<Yarp_Configuration.WebProxyConfig, WebProxyConfig>();

        CreateMap<ClusterConfig, Yarp_Configuration.ClusterConfig>();
        CreateMap<SessionAffinityConfig, Yarp_Configuration.SessionAffinityConfig>();
        CreateMap<HealthCheckConfig, Yarp_Configuration.HealthCheckConfig>();
        CreateMap<HttpClientConfig, Yarp_Configuration.HttpClientConfig>();
        CreateMap<ForwarderRequestConfig, Yarp_Forwarder.ForwarderRequestConfig>();
        CreateMap<DestinationConfig, Yarp_Configuration.DestinationConfig>();
        CreateMap<SessionAffinityCookieConfig, Yarp_Configuration.SessionAffinityCookieConfig>();
        CreateMap<ActiveHealthCheckConfig, Yarp_Configuration.ActiveHealthCheckConfig>();
        CreateMap<PassiveHealthCheckConfig, Yarp_Configuration.PassiveHealthCheckConfig>();
        CreateMap<WebProxyConfig, Yarp_Configuration.WebProxyConfig>();

        CreateMap<Yarp_Configuration.ClusterConfig, Yarp_Configuration.ClusterConfig>();
        CreateMap<Yarp_Configuration.SessionAffinityConfig, Yarp_Configuration.SessionAffinityConfig>();
        CreateMap<Yarp_Configuration.HealthCheckConfig, Yarp_Configuration.HealthCheckConfig>();
        CreateMap<Yarp_Configuration.HttpClientConfig, Yarp_Configuration.HttpClientConfig>();
        CreateMap<Yarp_Forwarder.ForwarderRequestConfig, Yarp_Forwarder.ForwarderRequestConfig>();
        CreateMap<Yarp_Configuration.DestinationConfig, Yarp_Configuration.DestinationConfig>();
        CreateMap<Yarp_Configuration.SessionAffinityCookieConfig, Yarp_Configuration.SessionAffinityCookieConfig>();
        CreateMap<Yarp_Configuration.ActiveHealthCheckConfig, Yarp_Configuration.ActiveHealthCheckConfig>();
        CreateMap<Yarp_Configuration.PassiveHealthCheckConfig, Yarp_Configuration.PassiveHealthCheckConfig>();
        CreateMap<Yarp_Configuration.WebProxyConfig, Yarp_Configuration.WebProxyConfig>();
    }
}
