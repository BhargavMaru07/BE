namespace WIMS.Domain.Entity;

public class ReorderAlert : BaseEntity
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal ReorderLevel { get; set; }
    public DateTime AlertDate { get; set; }
    public bool IsAcknowledged { get; set; } = false;
    public int? AcknowledgedBy { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public Product Product { get; set; } = default!;
    public Warehouse Warehouse { get; set; } = default!;
    public User? AcknowledgedByUser { get; set; }
}