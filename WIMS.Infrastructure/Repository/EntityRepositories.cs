using Microsoft.EntityFrameworkCore;
using WIMS.Application.Interfaces.Repositories;
using WIMS.Domain.Entity;
using WIMS.Domain.Enums;
using WIMS.Infrastructure.Data;

namespace WIMS.Infrastructure.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext db) : base(db) { }

    public async Task<User?> GetByEmailAsync(string email)
        => await _dbSet
            .Include(u => u.Warehouse)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

    public async Task<bool> IsEmailTakenAsync(string email, int? excludeUserId = null)
        => await _dbSet.AnyAsync(u =>
            u.Email.ToLower() == email.ToLower() &&
            (excludeUserId == null || u.Id != excludeUserId));

    public async Task<User?> GetUserByRefreshTokenAsync(string token) => await _dbSet.FirstOrDefaultAsync(u => u.RefreshTokenHash == token);
}

public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(AppDbContext db) : base(db) { }

}

public class ZoneRepository : GenericRepository<Zone>, IZoneRepository
{
    public ZoneRepository(AppDbContext db) : base(db) { }

    public async Task<List<Zone>> GetByWarehouseAsync(int warehouseId)
        => await _dbSet
            .AsNoTracking()
            .Where(z => z.WarehouseId == warehouseId)
            .OrderBy(z => z.Code)
            .ToListAsync();

    public async Task<bool> IsCodeTakenInWarehouseAsync(string code, int warehouseId, int? excludeId = null)
        => await _dbSet.AnyAsync(z =>
            z.Code == code &&
            z.WarehouseId == warehouseId &&
            (excludeId == null || z.Id != excludeId));
}

public class BinRepository : GenericRepository<Bin>, IBinRepository
{
    public BinRepository(AppDbContext db) : base(db) { }

    public async Task<List<Bin>> GetByZoneAsync(int zoneId)
        => await _dbSet
            .AsNoTracking()
            .Where(b => b.ZoneId == zoneId)
            .OrderBy(b => b.Code)
            .ToListAsync();

    public async Task<int> CountBinsInZoneAsync(int zoneId)
        => await _dbSet.CountAsync(b => b.ZoneId == zoneId);
}
public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(AppDbContext db) : base(db) { }

    public async Task<List<AuditLog>> GetByEntityAsync(string entityName, int entityId)
        => await _dbSet
            .AsNoTracking()
            .Where(a => a.EntityName == entityName && a.EntityId == entityId.ToString())
            .OrderByDescending(a => a.PerformedAt)
            .ToListAsync();
}