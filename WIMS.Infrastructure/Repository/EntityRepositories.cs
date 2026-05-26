using Microsoft.EntityFrameworkCore;
using WIMS.Application.Interfaces.Repositories;
using WIMS.Domain.Entity;
using WIMS.Domain.Enums;
using WIMS.Infrastructure.Data;

namespace WIMS.Infrastructure.Repository;

// ─────────────────────────────────────────────────────────────────────────
// Each class inherits GenericRepository<T> — gets ALL generic methods free.
// Only truly custom queries are implemented here.
// ─────────────────────────────────────────────────────────────────────────

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

    public async Task<User?> GetUserByRefreshTokenAsync(string token) => await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == token);
}

public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(AppDbContext db) : base(db) { }

    public async Task<bool> IsCodeTakenAsync(string code, int? excludeId = null)
        => await _dbSet.AnyAsync(w =>
            w.Code == code && (excludeId == null || w.Id != excludeId));
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

public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(AppDbContext db) : base(db) { }

    public async Task<bool> IsNameTakenAsync(string name, int? excludeId = null)
        => await _dbSet.AnyAsync(c =>
            c.Name.ToLower() == name.ToLower() &&
            (excludeId == null || c.Id != excludeId));
}

public class UnitsOfMeasureRepository : GenericRepository<UnitsOfMeasure>, IUnitsOfMeasureRepository
{
    public UnitsOfMeasureRepository(AppDbContext db) : base(db) { }

    public async Task<UnitsOfMeasure?> GetByAbbreviationAsync(string abbreviation)
        => await _dbSet.FirstOrDefaultAsync(u =>
            u.Abbreviation.ToLower() == abbreviation.ToLower());
}

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext db) : base(db) { }

    public async Task<bool> IsSkuTakenAsync(string sku, int? excludeId = null)
        => await _dbSet.AnyAsync(p =>
            p.Sku == sku && (excludeId == null || p.Id != excludeId));
}

public class StockRecordRepository : GenericRepository<StockRecord>, IStockRecordRepository
{
    public StockRecordRepository(AppDbContext db) : base(db) { }

    public async Task<StockRecord?> GetByProductAndBinAsync(int productId, int binId)
        => await _dbSet.FirstOrDefaultAsync(sr =>
            sr.ProductId == productId && sr.BinId == binId);

    public async Task<List<StockRecord>> GetByProductAndWarehouseAsync(int productId, int warehouseId)
        => await _dbSet
            .AsNoTracking()
            .Where(sr => sr.ProductId == productId && sr.WarehouseId == warehouseId)
            .ToListAsync();
}

public class StockMovementRepository : GenericRepository<StockMovement>, IStockMovementRepository
{
    public StockMovementRepository(AppDbContext db) : base(db) { }

    public async Task<List<StockMovement>> GetByReferenceAsync(string referenceType, int referenceId)
        => await _dbSet
            .AsNoTracking()
            .Where(m => m.ReferenceType == referenceType && m.ReferenceId == referenceId)
            .OrderByDescending(m => m.PerformedAt)
            .ToListAsync();
}

public class PurchaseOrderRepository : GenericRepository<PurchaseOrder>, IPurchaseOrderRepository
{
    public PurchaseOrderRepository(AppDbContext db) : base(db) { }

    public async Task<PurchaseOrder?> GetWithItemsAsync(int poId)
        => await _dbSet
            .Include(po => po.Items)
                .ThenInclude(i => i.Product)
            .Include(po => po.Warehouse)
            .FirstOrDefaultAsync(po => po.Id == poId);

    public async Task<bool> IsPoNumberTakenAsync(string poNumber)
        => await _dbSet.AnyAsync(po => po.PoNumber == poNumber);
}

public class PurchaseOrderItemRepository : GenericRepository<PurchaseOrderItem>, IPurchaseOrderItemRepository
{
    public PurchaseOrderItemRepository(AppDbContext db) : base(db) { }

    public async Task<List<PurchaseOrderItem>> GetByPoAsync(int poId)
        => await _dbSet
            .AsNoTracking()
            .Include(i => i.Product)
            .Where(i => i.PoId == poId)
            .ToListAsync();
}

public class GoodsReceiptRepository : GenericRepository<GoodsReceipt>, IGoodsReceiptRepository
{
    public GoodsReceiptRepository(AppDbContext db) : base(db) { }

    public async Task<GoodsReceipt?> GetWithItemsAsync(int grnId)
        => await _dbSet
            .Include(gr => gr.Items)
                .ThenInclude(i => i.Product)
            .Include(gr => gr.Items)
                .ThenInclude(i => i.Bin)
            .Include(gr => gr.PurchaseOrder)
            .Include(gr => gr.ReceivedByUser)
            .FirstOrDefaultAsync(gr => gr.Id == grnId);

    public async Task<List<GoodsReceipt>> GetByPoAsync(int poId)
        => await _dbSet
            .AsNoTracking()
            .Where(gr => gr.PoId == poId)
            .OrderByDescending(gr => gr.ReceiptDate)
            .ToListAsync();
}

public class GoodsReceiptItemRepository : GenericRepository<GoodsReceiptItem>, IGoodsReceiptItemRepository
{
    public GoodsReceiptItemRepository(AppDbContext db) : base(db) { }
}

public class GoodsDispatchRepository : GenericRepository<GoodsDispatch>, IGoodsDispatchRepository
{
    public GoodsDispatchRepository(AppDbContext db) : base(db) { }

    public async Task<GoodsDispatch?> GetWithItemsAsync(int gdnId)
        => await _dbSet
            .Include(gd => gd.Items)
                .ThenInclude(i => i.Product)
            .Include(gd => gd.Items)
                .ThenInclude(i => i.Bin)
            .Include(gd => gd.Warehouse)
            .FirstOrDefaultAsync(gd => gd.Id == gdnId);
}

public class GoodsDispatchItemRepository : GenericRepository<GoodsDispatchItem>, IGoodsDispatchItemRepository
{
    public GoodsDispatchItemRepository(AppDbContext db) : base(db) { }
}

public class StockTransferRepository : GenericRepository<StockTransfer>, IStockTransferRepository
{
    public StockTransferRepository(AppDbContext db) : base(db) { }

    public async Task<StockTransfer?> GetWithItemsAsync(int transferId)
        => await _dbSet
            .Include(st => st.Items)
                .ThenInclude(i => i.Product)
            .Include(st => st.Items)
                .ThenInclude(i => i.SourceBin)
            .Include(st => st.Items)
                .ThenInclude(i => i.DestBin)
            .Include(st => st.SourceWarehouse)
            .Include(st => st.DestWarehouse)
            .FirstOrDefaultAsync(st => st.Id == transferId);
}

public class StockTransferItemRepository : GenericRepository<StockTransferItem>, IStockTransferItemRepository
{
    public StockTransferItemRepository(AppDbContext db) : base(db) { }
}

public class StockAdjustmentRepository : GenericRepository<StockAdjustment>, IStockAdjustmentRepository
{
    public StockAdjustmentRepository(AppDbContext db) : base(db) { }

    public async Task<List<StockAdjustment>> GetPendingAsync()
        => await _dbSet
            .AsNoTracking()
            .Include(a => a.Product)
            .Include(a => a.Warehouse)
            .Include(a => a.Bin)
            .Where(a => a.ApprovalStatus == AdjustmentApprovalStatus.Pending)
            .OrderBy(a => a.CreatedAt)
            .ToListAsync();
}

public class ReorderAlertRepository : GenericRepository<ReorderAlert>, IReorderAlertRepository
{
    public ReorderAlertRepository(AppDbContext db) : base(db) { }

    public async Task<List<ReorderAlert>> GetUnacknowledgedAsync(int? warehouseId = null)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(r => r.Product)
            .Include(r => r.Warehouse)
            .Where(r => !r.IsAcknowledged);

        if (warehouseId.HasValue)
            query = query.Where(r => r.WarehouseId == warehouseId.Value);

        return await query.OrderByDescending(r => r.AlertDate).ToListAsync();
    }

    public async Task<bool> HasOpenAlertAsync(int productId, int warehouseId)
        => await _dbSet.AnyAsync(r =>
            r.ProductId == productId &&
            r.WarehouseId == warehouseId &&
            !r.IsAcknowledged);
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