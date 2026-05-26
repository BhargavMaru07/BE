using WIMS.Domain.Entity;

namespace WIMS.Application.Interfaces.Repositories;

// ─────────────────────────────────────────────────────────────────────────
// Every interface extends IGenericRepository<T>.
// This gives it ALL methods automatically:
//   GetAllAsync, GetAsync, GetPaginatedAsync (search+filter+sort+page),
//   CreateAsync, AddAsync, StageUpdateAsync, UpdateAsync, DeleteAsync,
//   SaveChangesAsync, ExistsAsync, CountAsync, GetQueryable,
//   BeginTransactionAsync, CommitTransactionAsync, RollbackTransactionAsync,
//   CreateSavepointAsync, RollbackToSavepointAsync, ReleaseSavepointAsync
//
// Only truly custom queries are added per interface below.
// ─────────────────────────────────────────────────────────────────────────

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetUserByRefreshTokenAsync(string token);
    Task<bool> IsEmailTakenAsync(string email, int? excludeUserId = null);
}

public interface IWarehouseRepository : IGenericRepository<Warehouse>
{
    Task<bool> IsCodeTakenAsync(string code, int? excludeId = null);
}

public interface IZoneRepository : IGenericRepository<Zone>
{
    Task<List<Zone>> GetByWarehouseAsync(int warehouseId);
    Task<bool> IsCodeTakenInWarehouseAsync(string code, int warehouseId, int? excludeId = null);
}

public interface IBinRepository : IGenericRepository<Bin>
{
    Task<List<Bin>> GetByZoneAsync(int zoneId);
    Task<int> CountBinsInZoneAsync(int zoneId);
}

public interface IProductCategoryRepository : IGenericRepository<ProductCategory>
{
    Task<bool> IsNameTakenAsync(string name, int? excludeId = null);
}

public interface IUnitsOfMeasureRepository : IGenericRepository<UnitsOfMeasure>
{
    Task<UnitsOfMeasure?> GetByAbbreviationAsync(string abbreviation);
}

public interface IProductRepository : IGenericRepository<Product>
{
    Task<bool> IsSkuTakenAsync(string sku, int? excludeId = null);
}

public interface IStockRecordRepository : IGenericRepository<StockRecord>
{
    Task<StockRecord?> GetByProductAndBinAsync(int productId, int binId);
    Task<List<StockRecord>> GetByProductAndWarehouseAsync(int productId, int warehouseId);
}

public interface IStockMovementRepository : IGenericRepository<StockMovement>
{
    Task<List<StockMovement>> GetByReferenceAsync(string referenceType, int referenceId);
}

public interface IPurchaseOrderRepository : IGenericRepository<PurchaseOrder>
{
    Task<PurchaseOrder?> GetWithItemsAsync(int poId);
    Task<bool> IsPoNumberTakenAsync(string poNumber);
}

public interface IPurchaseOrderItemRepository : IGenericRepository<PurchaseOrderItem>
{
    Task<List<PurchaseOrderItem>> GetByPoAsync(int poId);
}

public interface IGoodsReceiptRepository : IGenericRepository<GoodsReceipt>
{
    Task<GoodsReceipt?> GetWithItemsAsync(int grnId);
    Task<List<GoodsReceipt>> GetByPoAsync(int poId);
}

public interface IGoodsReceiptItemRepository : IGenericRepository<GoodsReceiptItem> { }

public interface IGoodsDispatchRepository : IGenericRepository<GoodsDispatch>
{
    Task<GoodsDispatch?> GetWithItemsAsync(int gdnId);
}

public interface IGoodsDispatchItemRepository : IGenericRepository<GoodsDispatchItem> { }

public interface IStockTransferRepository : IGenericRepository<StockTransfer>
{
    Task<StockTransfer?> GetWithItemsAsync(int transferId);
}

public interface IStockTransferItemRepository : IGenericRepository<StockTransferItem> { }

public interface IStockAdjustmentRepository : IGenericRepository<StockAdjustment>
{
    Task<List<StockAdjustment>> GetPendingAsync();
}

public interface IReorderAlertRepository : IGenericRepository<ReorderAlert>
{
    Task<List<ReorderAlert>> GetUnacknowledgedAsync(int? warehouseId = null);
    Task<bool> HasOpenAlertAsync(int productId, int warehouseId);
}

public interface IAuditLogRepository : IGenericRepository<AuditLog>
{
    Task<List<AuditLog>> GetByEntityAsync(string entityName, int entityId);
}