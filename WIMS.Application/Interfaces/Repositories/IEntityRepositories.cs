using WIMS.Domain.Entity;

namespace WIMS.Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetUserByRefreshTokenAsync(string token);
    Task<bool> IsEmailTakenAsync(string email, int? excludeUserId = null);
}

public interface IWarehouseRepository : IGenericRepository<Warehouse>
{
}

public interface IZoneRepository : IGenericRepository<Zone>
{
    Task<List<Zone>> GetByWarehouseAsync(int warehouseId);
}

public interface IBinRepository : IGenericRepository<Bin>
{
    Task<List<Bin>> GetByZoneAsync(int zoneId);
    Task<int> CountBinsInZoneAsync(int zoneId);
}

public interface IAuditLogRepository : IGenericRepository<AuditLog>
{
    Task<List<AuditLog>> GetByEntityAsync(string entityName, int entityId);
}