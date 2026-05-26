using WIMS.Domain.Enums;

namespace WIMS.Application.DTOs.Admin;

public class UpdateUserRoleRequest
{
    public required UserRole Role { get; set; }
    public int? WarehouseId { get; set; }
}