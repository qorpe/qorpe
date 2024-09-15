using AutoMapper;
using Qorpe.Application.Common.DTOs;
using Qorpe.Domain.Entities;
using Yarp_Configuration = Yarp.ReverseProxy.Configuration;

namespace Qorpe.Application.Common.Mappings;

public class RoutesMapping : Profile
{
    public RoutesMapping()
    {
        CreateMap<RouteConfig, RouteConfigDto>();
        CreateMap<RouteHeader, RouteHeaderDto>();
        CreateMap<RouteMatch, RouteMatchDto>();
        CreateMap<RouteQueryParameter, RouteQueryParameterDto>();

        CreateMap<RouteConfigDto, RouteConfig>();
        CreateMap<RouteHeaderDto, RouteHeader>();
        CreateMap<RouteMatchDto, RouteMatch>();
        CreateMap<RouteQueryParameterDto, RouteQueryParameter>();

        CreateMap<Yarp_Configuration.RouteConfig, RouteConfig>();
        CreateMap<Yarp_Configuration.RouteHeader, RouteHeader>();
        CreateMap<Yarp_Configuration.RouteMatch, RouteMatch>();
        CreateMap<Yarp_Configuration.RouteQueryParameter, RouteQueryParameter>();

        CreateMap<RouteConfig, Yarp_Configuration.RouteConfig>();
        CreateMap<RouteHeader, Yarp_Configuration.RouteHeader>();
        CreateMap<RouteMatch, Yarp_Configuration.RouteMatch>();
        CreateMap<RouteQueryParameter, Yarp_Configuration.RouteQueryParameter>();

        CreateMap<Yarp_Configuration.RouteConfig, Yarp_Configuration.RouteConfig>();
        CreateMap<Yarp_Configuration.RouteHeader, Yarp_Configuration.RouteHeader>();
        CreateMap<Yarp_Configuration.RouteMatch, Yarp_Configuration.RouteMatch>();
        CreateMap<Yarp_Configuration.RouteQueryParameter, Yarp_Configuration.RouteQueryParameter>();

        CreateMap<Yarp_Configuration.RouteConfig, RouteConfigDto>();
        CreateMap<Yarp_Configuration.RouteHeader, RouteHeaderDto>();
        CreateMap<Yarp_Configuration.RouteMatch, RouteMatchDto>();
        CreateMap<Yarp_Configuration.RouteQueryParameter, RouteQueryParameterDto>();
    }
}
