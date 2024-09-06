using AutoMapper;
using Qorpe.Application.Common.DTOs;
using Qorpe.Domain.Entities.Lite;

namespace Qorpe.Application.Common.Mappings;

public class RoutesMappingProfile : Profile
{
    public RoutesMappingProfile()
    {
        CreateMap<RouteConfig, RouteConfigDto>();
        CreateMap<RouteHeader, RouteHeaderDto>();
        CreateMap<RouteMatch, RouteMatchDto>();
        CreateMap<RouteQueryParameter, RouteQueryParameterDto>();

        CreateMap<RouteConfigDto, RouteConfig>();
        CreateMap<RouteHeaderDto, RouteHeader>();
        CreateMap<RouteMatchDto, RouteMatch>();
        CreateMap<RouteQueryParameterDto, RouteQueryParameter>();
    }
}
