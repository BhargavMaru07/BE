namespace WIMS.Domain.Entity;

public class AuditableEntity : BaseEntity
{
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
}
