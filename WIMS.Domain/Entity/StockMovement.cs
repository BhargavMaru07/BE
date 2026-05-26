using WIMS.Domain.Enums;

namespace WIMS.Domain.Entity;

public class StockMovement:IEntity
{
    public int Id { get; set; }
    public required int ProductId { get; set; }
    public required int WarehouseId { get; set; }
    public required int BinId { get; set; }
    public required StockMovementType MovementType { get; set; }
    public required decimal QuantityChange { get; set; }
    public required decimal QuantityBefore { get; set; }
    public required decimal QuantityAfter { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public required int PerformedBy { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public Product Product { get; set; } = default!;
    public Warehouse Warehouse { get; set; } = default!;
    public Bin Bin { get; set; } = default!;
    public User PerformedByUser { get; set; } = default!;

}
