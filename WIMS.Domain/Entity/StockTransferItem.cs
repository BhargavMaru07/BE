namespace WIMS.Domain.Entity;

public class StockTransferItem : BaseEntity
{
    public int TransferId { get; set; }
    public int ProductId { get; set; }
    public int SourceBinId { get; set; }
    public int DestBinId { get; set; }
    public decimal Quantity { get; set; }
    public StockTransfer StockTransfer { get; set; } = default!;
    public Product Product { get; set; } = default!;
    public Bin SourceBin { get; set; } = default!;
    public Bin DestBin { get; set; } = default!;
}
