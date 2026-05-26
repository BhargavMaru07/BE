namespace WIMS.Domain.Entity;

public class UnitsOfMeasure:BaseEntity
{
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
}
