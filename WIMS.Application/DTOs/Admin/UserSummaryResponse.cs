namespace WIMS.Application.DTOs.Admin;
public class UserSummaryResponse
{
    public int Id { get; set; }
    public required string FullName { get; set; } 
    public required string Email { get; set; } 
    public required string Role { get; set; }
    public required string Status { get; set; } 
    public string? WarehouseName { get; set; }
}
