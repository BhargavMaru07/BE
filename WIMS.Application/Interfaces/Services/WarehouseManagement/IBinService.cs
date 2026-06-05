using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Warehouse;

namespace WIMS.Application.Interfaces.Services.WarehouseManagement;

public interface IBinService
{
    Task<ApiResponse<BinResponse>> CreateBin(BinCreateRequest request, int createdByUserId);
    Task<ApiResponse<BinResponse>> GetBinById(int id);
    Task<ApiResponse<PagedResult<BinResponse>>> GetBins(QueryParameters qp);
    Task<ApiResponse<List<BinDropdownResponse>>> GetBinsDropdown(int? warehouseId = null, int? zoneId = null);
    Task<ApiResponse<BinResponse>> UpdateBin(int id, BinUpdateRequest request, int modifiedByUserId);
    Task<ApiResponse<string>> UpdateBinStatus(int id, BinStatusUpdateRequest request, int modifiedByUserId);
    Task<ApiResponse<string>> DeleteBin(int id, int deletedBy);
}
