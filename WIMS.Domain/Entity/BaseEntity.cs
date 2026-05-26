namespace WIMS.Domain.Entity;

public class BaseEntity: IEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
}
