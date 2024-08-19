using AutoMapper;
using Qorpe.Application.Common.DTOs;
using Qorpe.Domain.Entities;

namespace Qorpe.Application.Common.Mappings;

public class RoutesMappingProfile : Profile
{
    public RoutesMappingProfile()
    {
        CreateMap<RouteConfig, RouteConfigDto>();
        CreateMap<RouteConfigMetadata, RouteConfigMetadataDto>();
        CreateMap<RouteHeader, RouteHeaderDto>();
        CreateMap<RouteMatch, RouteMatchDto>();
        CreateMap<RouteQueryParameter, RouteQueryParameterDto>();
        CreateMap<TransformMetadata, TransformMetadataDto>();
        CreateMap<Transform, TransformDto>();

        CreateMap<RouteConfigDto, RouteConfig>();
        CreateMap<RouteConfigMetadataDto, RouteConfigMetadata>();
        CreateMap<RouteHeaderDto, RouteHeader>();
        CreateMap<RouteMatchDto, RouteMatch>();
        CreateMap<RouteQueryParameterDto, RouteQueryParameter>();
        CreateMap<TransformMetadataDto, TransformMetadata>();
        CreateMap<TransformDto, Transform>();
    }
}
