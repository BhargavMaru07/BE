namespace WIMS.Domain.Entity;

public class GoodsReceipt : BaseEntity
{
    public required string GrnNumber { get; set; }
    public int PoId { get; set; }
    public int WarehouseId { get; set; }
    public DateOnly ReceiptDate { get; set; }
    public int ReceivedBy { get; set; }
    public string? Notes { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; } = default!;
    public Warehouse Warehouse { get; set; } = default!;
    public User ReceivedByUser { get; set; } = default!;
    public ICollection<GoodsReceiptItem> Items { get; set; } = [];
}
