namespace WIMS.Domain.Entity;

public class GoodsDispatchItem:BaseEntity
{
    public int GdnId { get; set; }
    public int ProductId { get; set; }
    public int BinId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }           
 
    public GoodsDispatch GoodsDispatch { get; set; } = default!;
    public Product Product { get; set; } = default!;
    public Bin Bin { get; set; } = default!;
}
