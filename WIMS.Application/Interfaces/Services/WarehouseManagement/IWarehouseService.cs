using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Warehouse;

namespace WIMS.Application.Interfaces.Services.WarehouseManagement;

public interface IWarehouseService
{
    Task<ApiResponse<WarehouseResponse>> CreateWarehouse(WarehouseCreateRequest request, int createdByUserId);
    Task<ApiResponse<WarehouseResponse>> GetWarehouseByCode(string code);
    Task<ApiResponse<PagedResult<WarehouseResponse>>> GetWarehouses(QueryParameters qp);
    Task<ApiResponse<WarehouseResponse>> UpdateWarehouse(string code, WarehouseUpdateRequest request, int modifiedByUserId);
    Task<ApiResponse<string>> UpdateWarehouseStatus(string code,WarehouseStatusUpdateRequest request, int modifiedByUserId);
}
