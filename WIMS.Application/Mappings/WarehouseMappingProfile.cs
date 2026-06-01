using AutoMapper;
using WIMS.Application.DTOs.Warehouse;
using WIMS.Domain.Entity;

namespace WIMS.Application.Mappings;

public class WarehouseMappingProfile : Profile
{
    public WarehouseMappingProfile()
    {
        CreateMap<Warehouse, WarehouseResponse>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<WarehouseCreateRequest, Warehouse>()
            .ForMember(dest => dest.Code,
                opt => opt.MapFrom(src => "Temp"));
    }
}
