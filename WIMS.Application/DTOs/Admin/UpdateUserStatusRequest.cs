using WIMS.Domain.Enums;

namespace WIMS.Application.DTOs.Admin;
public class UpdateUserStatusRequest
{
    public required EntityStatus Status { get; set; }
}