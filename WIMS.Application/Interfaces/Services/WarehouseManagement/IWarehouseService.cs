using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Warehouse;

namespace WIMS.Application.Interfaces.Services.WarehouseManagement;

public interface IWarehouseService
{
    Task<ApiResponse<WarehouseResponse>> CreateWarehouse(WarehouseCreateRequest request, int createdByUserId);
    Task<ApiResponse<WarehouseResponse>> GetWarehouseById(int id);
    Task<ApiResponse<PagedResult<WarehouseResponse>>> GetWarehouses(QueryParameters qp);
    Task<ApiResponse<WarehouseResponse>> UpdateWarehouse(int id, WarehouseUpdateRequest request, int modifiedByUserId);
    Task<ApiResponse<string>> UpdateWarehouseStatus(int id,WarehouseStatusUpdateRequest request, int modifiedByUserId);
    Task<ApiResponse<string>> DeleteWarehouse(int id, int deletedBy);
}
