using WIMS.Domain.Enums;

namespace WIMS.Domain.Entity;

public class GoodsDispatch : AuditableEntity
{
    
    public required string GdnNumber { get; set; } 
    public int WarehouseId { get; set; }
    public required string CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public DateOnly DispatchDate { get; set; }
    public DispatchStatus Status { get; set; } = DispatchStatus.Draft;
    public int? ConfirmedBy { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public int? DispatchedBy { get; set; }
    public DateTime? DispatchedAt { get; set; }
    public Warehouse Warehouse { get; set; } = default!;
    public ICollection<GoodsDispatchItem> Items { get; set; } = [];
}
