using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WIMS.Domain.Entity;

namespace WIMS.Infrastructure.Data.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> b)
    {
        b.ToTable("product_categories");

        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.HasIndex(x => x.Name).IsUnique();
        b.Property(x => x.Description).HasMaxLength(500);
    }
}

public class UnitsOfMeasureConfiguration : IEntityTypeConfiguration<UnitsOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitsOfMeasure> b)
    {
        b.ToTable("units_of_measure");

        b.Property(x => x.Name).HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.Name).IsUnique();
        b.Property(x => x.Abbreviation).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Abbreviation).IsUnique();
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.ToTable("products");

        b.Property(x => x.Sku).HasMaxLength(30).IsRequired();
        b.HasIndex(x => x.Sku).IsUnique();
        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).HasMaxLength(1000);
        b.Property(x => x.UnitPrice).HasPrecision(14, 2).IsRequired();
        b.Property(x => x.ReorderLevel).HasPrecision(12, 2).IsRequired();

        b.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Uom)
            .WithMany()
            .HasForeignKey(x => x.UomId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
    }
}