namespace WIMS.Domain.Entity;

public class PurchaseOrderItem : AuditableEntity
{
    public int PoId { get; set; }
    public int ProductId { get; set; }
    public decimal OrderedQty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ReceivedQty { get; set; } = 0;         
    public decimal LineTotal { get; set; }         
    public PurchaseOrder PurchaseOrder { get; set; } = default!;
    public Product Product { get; set; } = default!;
}
