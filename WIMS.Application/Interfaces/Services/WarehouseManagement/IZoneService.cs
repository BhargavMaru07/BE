using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Warehouse;

namespace WIMS.Application.Interfaces.Services.WarehouseManagement;

public interface IZoneService
{
    Task<ApiResponse<ZoneResponse>> CreateZone(ZoneCreateRequest request, int createdByUserId);
    Task<ApiResponse<ZoneResponse>> GetZoneById(int id);
    Task<ApiResponse<PagedResult<ZoneResponse>>> GetZones(QueryParameters qp);
    Task<ApiResponse<List<ZoneDropdownResponse>>> GetZonesDropdown(int? warehouseId = null);
    Task<ApiResponse<ZoneResponse>> UpdateZone(int id, ZoneUpdateRequest request, int modifiedByUserId);
    Task<ApiResponse<string>> UpdateZoneStatus(int id, ZoneStatusUpdateRequest request, int modifiedByUserId);
    Task<ApiResponse<string>> DeleteZone(int id, int deletedBy);
}
