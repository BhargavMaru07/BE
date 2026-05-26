using WIMS.Domain.Enums;

namespace WIMS.Domain.Entity;

public class User : AuditableEntity
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public string? PhoneNumber { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.Active;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockedUntil { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
}
