using WIMS.Domain.Enums;

namespace WIMS.Domain.Entity;

public class StockAdjustment : BaseEntity
{
    public required string AdjustmentNumber { get; set; } 
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public int BinId { get; set; }
    public AdjustmentType AdjustmentType { get; set; }
    public decimal Quantity { get; set; }                 
    public AdjustmentReason Reason { get; set; }
    public string? ReasonNotes { get; set; }            
    public AdjustmentApprovalStatus ApprovalStatus { get; set; } = AdjustmentApprovalStatus.Pending;
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }
    public Warehouse Warehouse { get; set; } = default!;
    public Product Product { get; set; } = default!;
    public Bin Bin { get; set; } = default!;
    public User CreatedByUser { get; set; } = default!;
    public User? ReviewedByUser { get; set; }
}