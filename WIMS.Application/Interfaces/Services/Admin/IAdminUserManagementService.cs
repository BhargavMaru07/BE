using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Admin;

namespace WIMS.Application.Interfaces.Services.Admin;

public interface IAdminUserManagementService
{
    Task<ApiResponse<UserResponseDto>> CreateUser(CreateUserRequest request, int createdByUserId);
    Task<ApiResponse<PagedResult<UserSummaryResponse>>> GetUsers(QueryParameters qp);
    Task<ApiResponse<UserResponseDto>> GetUserById(int userId);
    Task<ApiResponse<UserResponseDto>> UpdateUserStatus(int userId, UpdateUserStatusRequest request, int modifiedByUserId);
    Task<ApiResponse<UserResponseDto>> UpdateUserRole(int userId, UpdateUserRoleRequest request, int modifiedByUserId);
    Task<ApiResponse<UserResponseDto>> UpdateUserWarehouse(int userId, UpdateUserWarehouseRequest request, int modifiedByUserId);
}
