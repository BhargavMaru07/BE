using WIMS.Domain.Enums;

namespace WIMS.Domain.Entity;

public class StockTransfer:AuditableEntity
{
     public required string TransferNumber { get; set; } 
    public int SourceWarehouseId { get; set; }
    public int DestWarehouseId { get; set; }
    public TransferStatus Status { get; set; } = TransferStatus.Initiated;
    public int InitiatedBy { get; set; }
    public DateTime InitiatedAt { get; set; }
    public int? ReceivedBy { get; set; }
    public DateTime? ReceivedAt { get; set; }
    public int? CancelledBy { get; set; }
    public DateTime? CancelledAt { get; set; }
    public Warehouse SourceWarehouse { get; set; } = default!;
    public Warehouse DestWarehouse { get; set; } = default!;
    public User InitiatedByUser { get; set; } = default!;
    public ICollection<StockTransferItem> Items { get; set; } = [];
}
