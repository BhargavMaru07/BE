using WIMS.Domain.Enums;

namespace WIMS.Domain.Entity;

public class GoodsReceiptItem:BaseEntity
{
    public int GrnId { get; set; }
    public int PoItemId { get; set; }
    public int ProductId { get; set; }
    public int BinId { get; set; }
    public decimal Quantity { get; set; }
    public ReceiptCondition Condition { get; set; }
    public GoodsReceipt GoodsReceipt { get; set; } = default!;
    public PurchaseOrderItem PoItem { get; set; } = default!;
    public Product Product { get; set; } = default!;
    public Bin Bin { get; set; } = default!;
}
