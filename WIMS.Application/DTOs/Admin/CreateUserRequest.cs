using WIMS.Domain.Enums;

namespace WIMS.Application.DTOs.Admin;

public class CreateUserRequest
{
    public required string FullName {get; set;}
    public required string Email {get; set;}
    public required string Password {get; set;}
    public required UserRole Role {get; set;}
    public int? WarehouseId {get; set;}
}
