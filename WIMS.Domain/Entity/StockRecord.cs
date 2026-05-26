namespace WIMS.Domain.Entity;

public class StockRecord: IEntity
{
    public int Id { get; set; }
    public required int ProductId { get; set; }
    public required int WarehouseId { get; set; }
    public required int BinId { get; set; }
    public required decimal Quantity { get; set; } = 0;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    public Product Product { get; set; } = default!;
    public Warehouse Warehouse { get; set; } = default!;
    public Bin Bin { get; set; } = default!;
}
